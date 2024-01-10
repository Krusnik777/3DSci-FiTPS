using UnityEngine;

namespace SciFiTPS
{
    public enum WeaponMode
    {
        Primary,
        Secondary
    }

    [CreateAssetMenu]
    public sealed class WeaponProperties : ScriptableObject
    {
        [SerializeField] private WeaponMode m_mode;
        public WeaponMode Mode => m_mode;

        [SerializeField] private Projectile m_projectilePrefab;
        public Projectile ProjectilePrefab => m_projectilePrefab;

        [SerializeField] private float m_rateOfFire;
        public float RateOfFire => m_rateOfFire;

        [SerializeField] private int m_energyUsage;
        public int EnergyUsage => m_energyUsage;

        [SerializeField] private int m_energyRegenPerSecond;
        public int EnergyRegenPerSecond => m_energyRegenPerSecond;

        [SerializeField] private int m_energyAmountToStartFire;
        public int EnergyAmountToStartFire => m_energyAmountToStartFire;

        [SerializeField] private float m_spreadShotRange;
        public float SpreadShotRange => m_spreadShotRange;

        [SerializeField] private float m_spreadShotDistanceFactor = 0.1f;
        public float SpreadShotDistanceFactor => m_spreadShotDistanceFactor;

        [SerializeField] private AudioClip m_launchSFX;
        public AudioClip LaunchSFX => m_launchSFX;
    }
}
