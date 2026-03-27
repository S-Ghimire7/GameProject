using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Ezereal
{
    public enum ManualGears
    {
        Reverse = 0,
        Neutral = 1,
        Gear1 = 2,
        Gear2 = 3,
        Gear3 = 4,
        Gear4 = 5,
        Gear5 = 6
    }

    public class EzerealCarController : MonoBehaviour
    {
        public Rigidbody vehicleRB;

        [Header("Wheels")]
        public WheelCollider frontLeftWheelCollider;
        public WheelCollider frontRightWheelCollider;
        public WheelCollider rearLeftWheelCollider;
        public WheelCollider rearRightWheelCollider;

        [Header("UI")]
        public TMP_Text currentGearTMP_UI;
        public TMP_Text currentSpeedTMP_UI;

        [Header("Ignition")]
        public AudioSource ignitionAudio;
        public AudioSource engineAudio;

        [Header("Controls")]
        public KeyCode ignitionKey = KeyCode.I;
        public KeyCode gearUpKey = KeyCode.LeftShift;
        public KeyCode gearDownKey = KeyCode.LeftControl;
        public KeyCode handbrakeKey = KeyCode.Space;

        [Header("Car Settings")]
        public float horsePower = 400f;
        public float brakePower = 4000f;
        public float maxSteeringAngle = 28f;

        [Header("Top Speed")]
        public float maxCarSpeed = 100f;

        [Header("Gear Ratios")]
        public float[] gearRatios = { -3.0f, 0f, 3.5f, 2.5f, 1.8f, 1.2f, 1.0f };

        [Header("Gear Speed Limits (KM/H)")]
        public float[] gearSpeedLimits =
        {
            40f,  // Reverse
            0f,   // Neutral
            20f,  // Gear 1  🔥 FIXED
            40f,  // Gear 2
            60f,  // Gear 3
            80f,  // Gear 4
            100f  // Gear 5
        };

        public float finalDrive = 2.8f;

        [Header("Engine")]
        public float maxRPM = 5500f;

        [Header("Grip")]
        public float traction = 1.5f;

        [Header("State")]
        public ManualGears currentGear = ManualGears.Neutral;
        public bool isStarted = false;

        public bool stationary;
        public bool InAir;

        float throttleInput;
        float brakeInput;
        float steerInput;

        float smoothThrottle;
        float rpm;

        bool handbrakeActive;

        void Awake()
        {
            if (vehicleRB == null)
                vehicleRB = GetComponent<Rigidbody>();
        }

        void Update()
        {
            HandleIgnition();
            HandleGears();
            HandleHandbrake();

            UpdateState();
            UpdateUI();
            UpdateEngineSound();
        }

        void FixedUpdate()
        {
            if (!isStarted)
            {
                StopCar();
                return;
            }

            ApplySteering();
            ApplyAcceleration();
            ApplyBraking();
        }

        void HandleIgnition()
        {
            if (Input.GetKeyDown(ignitionKey))
            {
                isStarted = !isStarted;

                if (ignitionAudio != null)
                    ignitionAudio.Play();

                if (!isStarted && engineAudio != null)
                    engineAudio.Stop();
            }
        }

        void HandleGears()
        {
            if (Input.GetKeyDown(gearUpKey) && (int)currentGear < 6)
                currentGear++;

            if (Input.GetKeyDown(gearDownKey) && (int)currentGear > 0)
                currentGear--;
        }

        void HandleHandbrake()
        {
            handbrakeActive = Input.GetKey(handbrakeKey);
        }

        void ApplyAcceleration()
        {
            if (!isStarted || handbrakeActive)
            {
                rearLeftWheelCollider.motorTorque = 0;
                rearRightWheelCollider.motorTorque = 0;
                return;
            }

            float speedKmh = vehicleRB.linearVelocity.magnitude * 3.6f;

            float gearRatio = gearRatios[(int)currentGear];

            // ❌ Neutral
            if (gearRatio == 0f)
                return;

            float maxSpeed = gearSpeedLimits[(int)currentGear];

            // 🔥 HARD GEAR LIMIT (FIXES GEAR 1 ISSUE)
            if (maxSpeed > 0f && speedKmh >= maxSpeed)
            {
                rearLeftWheelCollider.motorTorque = 0;
                rearRightWheelCollider.motorTorque = 0;

                vehicleRB.linearVelocity *= 0.98f; // slight drag

                return;
            }

            // 🔥 GLOBAL SPEED LIMIT
            if (speedKmh >= maxCarSpeed)
            {
                rearLeftWheelCollider.motorTorque = 0;
                rearRightWheelCollider.motorTorque = 0;
                return;
            }

            // RPM calculation
            rpm = Mathf.Clamp(speedKmh * Mathf.Abs(gearRatio) * 80f, 0f, maxRPM);

            float throttle = Mathf.Clamp01(throttleInput);
            smoothThrottle = Mathf.Lerp(smoothThrottle, throttle, Time.deltaTime * 2f);

            float torque = horsePower * smoothThrottle * gearRatio * finalDrive;

            torque *= traction;

            rearLeftWheelCollider.motorTorque = torque;
            rearRightWheelCollider.motorTorque = torque;
        }

        void ApplyBraking()
        {
            float brake = brakePower * brakeInput;

            frontLeftWheelCollider.brakeTorque = brake;
            frontRightWheelCollider.brakeTorque = brake;

            rearLeftWheelCollider.brakeTorque = brake;
            rearRightWheelCollider.brakeTorque = brake;

            if (handbrakeActive)
            {
                rearLeftWheelCollider.brakeTorque = brakePower * 2f;
                rearRightWheelCollider.brakeTorque = brakePower * 2f;
            }
        }

        void ApplySteering()
        {
            float steer = maxSteeringAngle * steerInput;

            frontLeftWheelCollider.steerAngle = steer;
            frontRightWheelCollider.steerAngle = steer;
        }

        void StopCar()
        {
            rearLeftWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
        }

        void UpdateEngineSound()
        {
            if (!isStarted || engineAudio == null) return;

            engineAudio.volume = 1f;

            float speed = vehicleRB.linearVelocity.magnitude * 3.6f;
            float pitch = Mathf.Lerp(0.8f, 1.3f, speed / maxCarSpeed);

            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, pitch, Time.deltaTime * 2f);
        }

        void UpdateUI()
        {
            float speed = vehicleRB.linearVelocity.magnitude * 3.6f;

            if (currentSpeedTMP_UI != null)
                currentSpeedTMP_UI.text = speed.ToString("F0");

            if (currentGearTMP_UI != null)
                currentGearTMP_UI.text = currentGear.ToString();
        }

        // ✅ FIXED STATE SYSTEM
        void UpdateState()
        {
            float speed = vehicleRB.linearVelocity.magnitude;

            stationary = speed < 0.2f &&
                         Mathf.Abs(throttleInput) < 0.05f &&
                         Mathf.Abs(brakeInput) < 0.05f;

            bool wheelsGrounded =
                frontLeftWheelCollider.isGrounded ||
                frontRightWheelCollider.isGrounded ||
                rearLeftWheelCollider.isGrounded ||
                rearRightWheelCollider.isGrounded;

            bool groundCheck = Physics.Raycast(transform.position, Vector3.down, 1.5f);

            InAir = !wheelsGrounded && !groundCheck;
        }

        void OnAccelerate(InputValue value) => throttleInput = value.Get<float>();
        void OnBrake(InputValue value) => brakeInput = value.Get<float>();
        void OnSteer(InputValue value) => steerInput = value.Get<float>();
    }
}