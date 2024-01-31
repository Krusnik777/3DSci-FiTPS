using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponMode m_mode;
        public WeaponMode Mode => m_mode;
        [SerializeField] private WeaponProperties m_weaponProperties;
        [SerializeField] private Transform m_firePoint;
        [SerializeField] private ParticleSystem m_muzzleParticleSystem;
        [SerializeField] private float m_primaryMaxEnergy;
        public float PrimaryMaxEnergy => m_primaryMaxEnergy;

        private float m_refireTimer;
        private float m_primaryEnergy;
        public float PrimaryEnergy => m_primaryEnergy;
        private bool energyIsRestored;

        public bool CanFire => m_refireTimer <= 0 && energyIsRestored == false;

        private Destructible m_owner;

        private AudioSource m_audioSource;

        #region PublicAPI

        public void Fire()
        {
            if (!CanFire) return;

            if (m_weaponProperties == null) return;

            if (m_refireTimer > 0) return;
            
            if (TryDrawEnergy(m_weaponProperties.EnergyUsage) == false) return;

            Projectile projectile = Instantiate(m_weaponProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = m_firePoint.position;
            projectile.transform.forward = m_firePoint.forward;

            projectile.SetParentShooter(m_owner);

            m_refireTimer = m_weaponProperties.RateOfFire;

            if (m_weaponProperties.LaunchSFX != null)
            {
                if (m_muzzleParticleSystem != null)
                {
                    m_muzzleParticleSystem.time = 0;
                    m_muzzleParticleSystem.Play();
                }

                m_audioSource.clip = m_weaponProperties.LaunchSFX;
                m_audioSource.Play();
            }

        }

        public void FirePointLookAt(Vector3 pos)
        {
            Vector3 offset = Random.insideUnitSphere * m_weaponProperties.SpreadShotRange;

            if (m_weaponProperties.SpreadShotDistanceFactor != 0)
            {
                offset = offset * Vector3.Distance(m_firePoint.position, pos) * m_weaponProperties.SpreadShotDistanceFactor;
            }

            m_firePoint.LookAt(pos + offset);
        }

        public void AssignLoadout(WeaponProperties props)
        {
            if (m_mode != props.Mode) return;

            m_refireTimer = 0;
            m_weaponProperties = props;
        }

        #endregion

        private void Start()
        {
            m_owner = transform.root.GetComponent<Destructible>();
            m_audioSource = GetComponent<AudioSource>();
            m_primaryEnergy = m_primaryMaxEnergy;
        }

        protected virtual void Update()
        {
            if (m_refireTimer > 0)
                m_refireTimer -= Time.deltaTime;

            UpdateEnergy();
        }

        private void UpdateEnergy()
        {
            m_primaryEnergy += (float)m_weaponProperties.EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_primaryEnergy = Mathf.Clamp(m_primaryEnergy, 0, m_primaryMaxEnergy);

            if (m_primaryEnergy >= m_weaponProperties.EnergyAmountToStartFire)
                energyIsRestored = false;
        }

        private bool TryDrawEnergy(int count)
        {
            if (count == 0) return true;

            if (m_primaryEnergy >= count)
            {
                m_primaryEnergy -= count;
                return true;
            }

            energyIsRestored = true;

            return false;
        }
    }
}
