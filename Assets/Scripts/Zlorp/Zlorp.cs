using UnityEngine;

namespace SciFiTPS
{
    public class Zlorp : Destructible
    {
        [SerializeField] private Weapon m_weapon;
        [SerializeField] private SpreadShootRig m_spreadShootRig;

        public void Fire(Vector3 target)
        {
            if (!m_weapon.CanFire) return;

            m_weapon.FirePointLookAt(target);
            m_weapon.Fire();
            m_spreadShootRig.Spread();
        }

        protected override void OnDeath()
        {
            m_eventOnDeath?.Invoke();
        }
    }
}
