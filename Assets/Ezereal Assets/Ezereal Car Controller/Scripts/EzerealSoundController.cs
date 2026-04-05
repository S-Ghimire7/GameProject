using UnityEngine;

namespace Ezereal
{
    public class EzerealSoundController : MonoBehaviour
    {
        [SerializeField] bool useSounds = true;
        [SerializeField] EzerealCarController ezerealCarController;
        [SerializeField] AudioSource engineAudio;
        [SerializeField] AudioSource ignitionAudio;

        public float maxVolume = 0.5f;

        bool engineStartedAfterIgnition;

        void Start()
        {
            if (!useSounds) return;

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

            HandleEngineSound();
        }

        void HandleEngineSound()
        {
            if (engineAudio == null) return;

            float speed = GetSpeed();

            if (ezerealCarController.isStarted)
            {
                if (!engineStartedAfterIgnition)
                {
                    if (ignitionAudio != null && ignitionAudio.isPlaying)
                    {
                        Invoke(nameof(StartEngine), ignitionAudio.clip.length);
                        engineStartedAfterIgnition = true;
                    }
                    else
                    {
                        StartEngine();
                        engineStartedAfterIgnition = true;
                    }
                }
            }
            else
            {
                engineStartedAfterIgnition = false;

                if (engineAudio.isPlaying)
                    engineAudio.Stop();

                return;
            }

            float targetPitch = 0.6f + (speed / 40f);
            engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.deltaTime * 3f);

            float targetVolume = Mathf.Clamp01(speed / 30f);
            engineAudio.volume = Mathf.Lerp(engineAudio.volume, targetVolume, Time.deltaTime * 3f);
        }

        void StartEngine()
        {
            if (engineAudio != null && !engineAudio.isPlaying)
                engineAudio.Play();
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