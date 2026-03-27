using UnityEngine;

namespace Ezereal
{
    public class EzerealSoundController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] bool useSounds = true;
        [SerializeField] EzerealCarController ezerealCarController;
        [SerializeField] AudioSource tireAudio;
        [SerializeField] AudioSource engineAudio;

        [Header("Settings")]
        public float maxVolume = 0.5f;

        [Header("Debug")]
        [SerializeField] bool alreadyPlaying;

        void Start()
        {
            if (!useSounds) return;

            if (tireAudio != null)
            {
                tireAudio.volume = 0f;
                tireAudio.loop = true;
                tireAudio.Stop();
            }

            if (engineAudio != null)
            {
                engineAudio.loop = true;
                engineAudio.Stop();
            }
        }

        void Update()
        {
            if (!useSounds) return;

            if (ezerealCarController == null || ezerealCarController.vehicleRB == null)
                return;

            HandleTireSound();
            HandleEngineSound();
        }

        void HandleTireSound()
        {
            float speed = GetSpeed();

            // 🎯 SOUND CONDITIONS (FIXED)
            bool shouldPlay = !ezerealCarController.stationary && !ezerealCarController.InAir;

            if (shouldPlay)
            {
                if (!alreadyPlaying && tireAudio != null)
                {
                    tireAudio.Play();
                    alreadyPlaying = true;
                }
            }
            else
            {
                if (alreadyPlaying && tireAudio != null)
                {
                    tireAudio.Stop();
                    alreadyPlaying = false;
                }
            }

            // 🔊 VOLUME BASED ON SPEED
            if (tireAudio != null)
            {
                float targetVolume = Mathf.Clamp01(speed / 15f) * maxVolume;
                tireAudio.volume = Mathf.Lerp(tireAudio.volume, targetVolume, Time.deltaTime * 5f);

                float pitch = 0.8f + (speed / 50f);
                tireAudio.pitch = Mathf.Clamp(pitch, 0.8f, 2f);
            }
        }

        void HandleEngineSound()
        {
            if (engineAudio == null) return;

            float speed = GetSpeed();

            // 🔑 STOP ENGINE IF IGNITION OFF
            if (!ezerealCarController.isStarted)
            {
                if (engineAudio.isPlaying)
                    engineAudio.Stop();
                return;
            }

            if (!engineAudio.isPlaying)
                engineAudio.Play();

            // 🔊 ENGINE PITCH (SMOOTH + LOWER)
            float targetPitch = 0.6f + (speed / 40f);
            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.deltaTime * 3f);

            // 🔊 ENGINE VOLUME
            float targetVolume = Mathf.Clamp01(speed / 30f);
            engineAudio.volume = Mathf.Lerp(engineAudio.volume, targetVolume, Time.deltaTime * 3f);
        }

        float GetSpeed()
        {
            Rigidbody rb = ezerealCarController.vehicleRB;

#if UNITY_6000_0_OR_NEWER
            return rb.linearVelocity.magnitude;
#else
            return rb.velocity.magnitude;
#endif
        }
    }
}