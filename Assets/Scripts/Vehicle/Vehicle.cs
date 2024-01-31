using UnityEngine;

namespace SciFiTPS
{
    public class Vehicle : Destructible
    {
        [SerializeField] protected float m_maxLinearSpeed;
        [Header("EngineSound")]
        [SerializeField] private AudioSource m_engineSFX;
        [SerializeField] private float m_engineSFXModifier;

        protected Vector3 m_targetInputControl;

        private bool isEnabled = false;

        public virtual float LinearVelocity => 0;

        public float NormalizedLinearVelocity
        {
            get
            {
                if (Mathf.Approximately(0, LinearVelocity)) return 0;

                return Mathf.Clamp01(LinearVelocity / m_maxLinearSpeed);
            }
        }

        public void SetTargetControl(Vector3 control) => m_targetInputControl = control.normalized;

        public void EnableSFX(bool state)
        {
            isEnabled = state;
        }

        protected virtual void Update()
        {
            UpdateEngineSFX();
        }

        private void UpdateEngineSFX()
        {
            if (isEnabled)
            {
                if (m_engineSFX != null)
                {
                    m_engineSFX.pitch = 1.0f + NormalizedLinearVelocity * m_engineSFXModifier;
                    m_engineSFX.volume = 0.5f + NormalizedLinearVelocity;
                }
            }
            else
            {
                m_engineSFX.volume = 0.0f;
            }
        }

    }
}
