using UnityEngine;

namespace SciFiTPS
{
    public class Vanguard : Destructible
    {
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private float m_fallDamageFactor;

        protected override void Start()
        {
            base.Start();

            m_characterMovement.EventOnLand += OnLand;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_characterMovement.EventOnLand -= OnLand;
        }

        protected override void OnDeath()
        {
            m_characterMovement.EventOnLand -= OnLand;

            m_eventOnDeath?.Invoke();
        }

        private void OnLand(Vector3 vel)
        {
            if (Mathf.Abs(vel.y) < 10) return;

            if (Mathf.Abs(vel.y) > 20)
            {
                ApplyDamage(this, 9999);
            }

            ApplyDamage(this, (int) (Mathf.Abs(vel.y) * m_fallDamageFactor));
        }
    }
}
