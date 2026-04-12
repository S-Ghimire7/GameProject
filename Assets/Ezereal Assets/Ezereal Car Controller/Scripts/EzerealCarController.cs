using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

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

        public WheelCollider frontLeftWheelCollider;
        public WheelCollider frontRightWheelCollider;
        public WheelCollider rearLeftWheelCollider;
        public WheelCollider rearRightWheelCollider;

        public TMP_Text currentGearTMP_UI;
        public TMP_Text currentSpeedTMP_UI;

        public AudioSource ignitionAudio;
        public AudioSource engineAudio;
        public AudioSource shutdownAudio;

        public KeyCode ignitionKey = KeyCode.I;
        public KeyCode gearUpKey = KeyCode.LeftShift;
        public KeyCode gearDownKey = KeyCode.LeftControl;
        public KeyCode handbrakeKey = KeyCode.Space;
        public KeyCode clutchKey = KeyCode.LeftAlt;

        public float horsePower = 400f;
        public float brakePower = 4000f;
        public float maxSteeringAngle = 28f;
        public float maxCarSpeed = 130f;

        public float[] gearRatios = { -3.0f, 0f, 3.5f, 2.5f, 1.8f, 1.2f, 1.0f };
        public float[] gearSpeedLimits = { 40f, 0f, 15f, 35f, 65f, 90f, 110f };

        public float finalDrive = 2.8f;
        public float traction = 1.5f;

        public ManualGears currentGear = ManualGears.Neutral;

        public bool isStarted = false;

        float throttleInput;
        float brakeInput;
        float steerInput;

        bool clutchPressed;
        bool handbrakeActive;

        public Renderer handbrakeRenderer;
        public Renderer clutchRenderer;

        void Awake()
        {
            if (vehicleRB == null)
                vehicleRB = GetComponent<Rigidbody>();

            FindHandbrakeObject();
            FindClutchObject();
        }

        void Update()
        {
            HandleIgnition();
            HandleClutch();
            HandleGears();
            HandleHandbrake();
            UpdateUI();
            UpdateEngineSound();
        }

        void FixedUpdate()
        {
            if (!isStarted)
            {
                StopTorque();
                return;
            }

            ApplySteering();
            ApplyAcceleration();
            ApplyBraking();
        }

        void FindHandbrakeObject()
        {
            GameObject obj = GameObject.Find("Interior Light Handbrake");

            if (obj != null)
            {
                handbrakeRenderer = obj.GetComponent<Renderer>();
                if (handbrakeRenderer != null)
                    handbrakeRenderer.enabled = false;
            }
        }

        void FindClutchObject()
        {
            GameObject obj = GameObject.Find("Interior Light Clutch");

            if (obj != null)
            {
                clutchRenderer = obj.GetComponent<Renderer>();
                if (clutchRenderer != null)
                    clutchRenderer.enabled = false;
            }
        }

        void HandleIgnition()
        {
            if (Input.GetKeyDown(ignitionKey))
            {
                isStarted = !isStarted;

                if (isStarted && ignitionAudio != null)
                    ignitionAudio.Play();
                else
                {
                    if (engineAudio != null) engineAudio.Stop();
                    if (shutdownAudio != null) shutdownAudio.Play();
                    StopCar();
                }
            }
        }

        void HandleClutch()
        {
            clutchPressed = Input.GetKey(clutchKey);

            if (clutchRenderer != null)
                clutchRenderer.enabled = clutchPressed;
        }

        void HandleGears()
        {
            if (!clutchPressed) return;

            int previous = (int)currentGear;

            if (Input.GetKeyDown(gearUpKey) && (int)currentGear < 6)
                currentGear++;

            if (Input.GetKeyDown(gearDownKey) && (int)currentGear > 0)
                currentGear--;

            if ((int)currentGear != previous)
            {
                StartCoroutine(GearLockBrake());
            }
        }

        IEnumerator GearLockBrake()
        {
            float target = gearSpeedLimits[(int)currentGear];

            while (target > 0f && vehicleRB.linearVelocity.magnitude * 3.6f > target)
            {
                vehicleRB.linearVelocity *= 0.9f;
                yield return new WaitForFixedUpdate();
            }
        }

        void HandleHandbrake()
        {
            handbrakeActive = Input.GetKey(handbrakeKey);

            if (handbrakeRenderer != null)
                handbrakeRenderer.enabled = handbrakeActive;
        }

        void ApplyAcceleration()
        {
            float speedKmh = vehicleRB.linearVelocity.magnitude * 3.6f;

            float ratio = gearRatios[(int)currentGear];
            float limit = gearSpeedLimits[(int)currentGear];

            if (handbrakeActive || currentGear == ManualGears.Neutral)
            {
                ApplyBrakeForce(1f);
                StopTorque();
                return;
            }

            if (limit > 0f && speedKmh >= limit)
            {
                ApplyBrakeForce(1.5f);
                StopTorque();
                return;
            }

            if (Mathf.Abs(throttleInput) < 0.01f)
            {
                ApplyBrakeForce(2f);
                HandleCreep(speedKmh);
                return;
            }

            float torque = horsePower * throttleInput * ratio * finalDrive * traction;

            rearLeftWheelCollider.motorTorque = torque;
            rearRightWheelCollider.motorTorque = torque;

            ApplyBrakeForce(0.5f);
        }

        void HandleCreep(float speedKmh)
        {
            if (clutchPressed) return;

            if (currentGear == ManualGears.Gear1 && throttleInput < 0.01f && speedKmh < 5f)
            {
                float creep = horsePower * 0.05f;
                rearLeftWheelCollider.motorTorque = creep;
                rearRightWheelCollider.motorTorque = creep;
            }

            if (currentGear == ManualGears.Reverse && throttleInput < 0.01f && speedKmh < 5f)
            {
                float creep = horsePower * 0.05f;
                rearLeftWheelCollider.motorTorque = -creep;
                rearRightWheelCollider.motorTorque = -creep;
            }
        }

        void ApplyBrakeForce(float multiplier)
        {
            float speed = vehicleRB.linearVelocity.magnitude;

            float brake = Mathf.Clamp(speed * 1200f, 2000f, 30000f);
            brake *= multiplier;

            rearLeftWheelCollider.brakeTorque = brake;
            rearRightWheelCollider.brakeTorque = brake;
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
                rearLeftWheelCollider.brakeTorque = brakePower * 3f;
                rearRightWheelCollider.brakeTorque = brakePower * 3f;
            }
        }

        void ApplySteering()
        {
            float steer = maxSteeringAngle * steerInput;

            frontLeftWheelCollider.steerAngle = steer;
            frontRightWheelCollider.steerAngle = steer;
        }

        void StopTorque()
        {
            rearLeftWheelCollider.motorTorque = 0;
            rearRightWheelCollider.motorTorque = 0;
        }

        void StopCar()
        {
            StopTorque();
            vehicleRB.linearVelocity = Vector3.zero;
            vehicleRB.angularVelocity = Vector3.zero;
        }

        void UpdateEngineSound()
        {
            if (!isStarted || engineAudio == null) return;

            if (!engineAudio.isPlaying)
                engineAudio.Play();

            float speed = vehicleRB.linearVelocity.magnitude * 3.6f;
            engineAudio.pitch = Mathf.Lerp(0.8f, 1.3f, speed / maxCarSpeed);
        }

        void UpdateUI()
        {
            float speed = vehicleRB.linearVelocity.magnitude * 3.6f;

            if (currentSpeedTMP_UI != null)
                currentSpeedTMP_UI.text = speed.ToString("F0");

            if (currentGearTMP_UI != null)
                currentGearTMP_UI.text = currentGear.ToString();
        }

        void OnAccelerate(InputValue value) => throttleInput = value.Get<float>();
        void OnBrake(InputValue value) => brakeInput = value.Get<float>();
        void OnSteer(InputValue value) => steerInput = value.Get<float>();
    }
}