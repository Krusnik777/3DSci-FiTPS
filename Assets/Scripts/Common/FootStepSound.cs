using UnityEngine;

namespace SciFiTPS
{
    [System.Serializable]
    public class FootStepProperties
    {
        public float Speed;
        public float Delay;
    }

    public class FootStepSound : MonoBehaviour
    {
        [SerializeField] private FootStepProperties[] m_properties;
        [SerializeField] private CharacterController m_characterController;
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private NoiseAudioSource m_noiseAudioSource;

        private float m_delay;

        private float m_tick;

        private float GetSpeed() => m_characterController.velocity.magnitude;
        private float GetDelayBySpeed(float speed)
        {
            for (int i = 0; i < m_properties.Length; i++)
            {
                if (speed <= m_properties[i].Speed)
                {
                    return m_properties[i].Delay;
                }
            }

            return m_properties[m_properties.Length - 1].Delay;
        }

        private bool IsPlay()
        {
            if (GetSpeed() < 0.01f || !m_characterMovement.IsGrounded)
                return false;
            else
                return true;
        }

        private void Update()
        {
            if (!IsPlay()) 
            {
                m_tick = 0;
                return; 
            }

            m_tick += Time.deltaTime;

            m_delay = GetDelayBySpeed(GetSpeed());
            
            if (m_tick >= m_delay)
            {
                m_noiseAudioSource.Play();
                m_tick = 0;
            }

        }

    }
}
