using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(AudioSource))]
    public class NoiseAudioSource : MonoBehaviour
    {
        private AudioSource audioSource;

        public AudioClip clip { get => audioSource.clip; set => audioSource.clip = value; }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            audioSource.Play();

            foreach (var dest in Destructible.AllDestructibles)
            {
                if (dest is ISoundListener)
                {
                    (dest as ISoundListener).Heard(Vector3.Distance(transform.position, dest.transform.position));
                }
            }
        }
    }
}
