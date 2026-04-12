using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Ezereal
{
    public class EzerealLightController : MonoBehaviour
    {
        [Header("Beam Lights")]
        [Header("Sound")]

        [SerializeField] AudioSource indicatorAudioSource;
        [SerializeField] AudioClip indicatorSound;

        [SerializeField] AudioSource hazardAudioSource;
        [SerializeField] AudioClip hazardSound;

        [SerializeField] LightBeam currentBeam = LightBeam.off;

        [SerializeField] GameObject[] lowBeamHeadlights;
        [SerializeField] GameObject[] highBeamHeadlights;
        [SerializeField] GameObject[] lowBeamSpotlights;
        [SerializeField] GameObject[] highBeamSpotlights;
        [SerializeField] GameObject[] rearLights;

        [Header("Brake Lights")]
        [SerializeField] GameObject[] brakeLights;

        [Header("Handbrake Light")]
        [SerializeField] GameObject[] handbrakeLight;

        [Header("Reverse Lights")]
        [SerializeField] GameObject[] reverseLights;

        [Header("Turn Lights")]
        [SerializeField] GameObject[] leftTurnLights;
        [SerializeField] GameObject[] rightTurnLights;

        [Header("Misc Lights")]
        [SerializeField] GameObject[] miscLights;

        [Header("Settings")]
        [SerializeField] float lightBlinkDelay = 0.5f;

        [Header("Debug")]
        [SerializeField] bool leftTurnActive = false;
        [SerializeField] bool rightTurnActive = false;
        [SerializeField] bool hazardLightsActive = false;

        private void Start()
        {
            AllLightsOff();
        }

        public void AllLightsOff()
        {
            AllBeamsOff();
            ReverseLightsOff();
            TurnLightsOff();
            BrakeLightsOff();
        }

        void OnLowBeamLight()
        {
            switch (currentBeam)
            {
                case LightBeam.off:
                    LowBeamOn();
                    break;
                case LightBeam.low:
                case LightBeam.high:
                    AllBeamsOff();
                    break;
            }
        }

        void OnHighBeamLight()
        {
            switch (currentBeam)
            {
                case LightBeam.off:
                case LightBeam.low:
                    HighBeamOn();
                    break;
                case LightBeam.high:
                    AllBeamsOff();
                    break;
            }
        }

        void OnLeftTurnSignal()
        {
            if (!hazardLightsActive)
            {
                StopAllCoroutines();
                TurnLightsOff();
                rightTurnActive = false;
                leftTurnActive = !leftTurnActive;

                if (leftTurnActive)
                    StartCoroutine(TurnSignalController(leftTurnLights, leftTurnActive));
            }
        }

        void OnRightTurnSignal()
        {
            if (!hazardLightsActive)
            {
                StopAllCoroutines();
                TurnLightsOff();
                leftTurnActive = false;
                rightTurnActive = !rightTurnActive;

                if (rightTurnActive)
                    StartCoroutine(TurnSignalController(rightTurnLights, rightTurnActive));
            }
        }

        void OnHazardLights()
        {
            StopAllCoroutines();
            TurnLightsOff();
            leftTurnActive = false;
            rightTurnActive = false;
            hazardLightsActive = !hazardLightsActive;

            if (hazardLightsActive)
                StartCoroutine(HazardLightsController());
        }

        IEnumerator TurnSignalController(GameObject[] turnLights, bool isActive)
        {
            while (isActive)
            {
                SetLight(turnLights, true);

                if (indicatorAudioSource && indicatorSound)
                    indicatorAudioSource.PlayOneShot(indicatorSound);

                yield return new WaitForSeconds(lightBlinkDelay);

                SetLight(turnLights, false);
                yield return new WaitForSeconds(lightBlinkDelay);
            }
        }

        IEnumerator HazardLightsController()
        {
            while (hazardLightsActive)
            {
                TurnLightsOn();

                if (hazardAudioSource && hazardSound && !hazardAudioSource.isPlaying)
                    hazardAudioSource.PlayOneShot(hazardSound);

                yield return new WaitForSeconds(lightBlinkDelay);

                TurnLightsOff();
                yield return new WaitForSeconds(lightBlinkDelay);
            }

            if (hazardAudioSource && hazardAudioSource.isPlaying)
                hazardAudioSource.Stop();
        }

        void SetLight(GameObject[] lights, bool isActive)
        {
            foreach (var light in lights)
                light.SetActive(isActive);
        }

        void AllBeamsOff()
        {
            SetLight(rearLights, false);
            currentBeam = LightBeam.off;
        }

        void LowBeamOn()
        {
            SetLight(lowBeamHeadlights, true);
            SetLight(lowBeamSpotlights, true);
            SetLight(rearLights, true);
            SetLight(highBeamHeadlights, false);
            SetLight(highBeamSpotlights, false);
            currentBeam = LightBeam.low;
        }

        void HighBeamOn()
        {
            SetLight(lowBeamHeadlights, true);
            SetLight(lowBeamSpotlights, false);
            SetLight(rearLights, true);
            SetLight(highBeamHeadlights, true);
            SetLight(highBeamSpotlights, true);
            currentBeam = LightBeam.high;
        }

        void TurnLightsOff()
        {
            SetLight(leftTurnLights, false);
            SetLight(rightTurnLights, false);
        }

        void TurnLightsOn()
        {
            SetLight(leftTurnLights, true);
            SetLight(rightTurnLights, true);
        }

        public void BrakeLightsOff()
        {
            SetLight(brakeLights, false);
        }

        public void BrakeLightsOn()
        {
            SetLight(brakeLights, true);
        }

        public void ReverseLightsOff()
        {
            SetLight(reverseLights, false);
        }

        public void ReverseLightsOn()
        {
            SetLight(reverseLights, true);
        }

        public void MiscLightsOff()
        {
            SetLight(miscLights, false);
        }

        public void MiscLightsOn()
        {
            SetLight(miscLights, true);
        }
    }
}