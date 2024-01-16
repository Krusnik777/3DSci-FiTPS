using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    /// <summary>
    /// Destructible object on scene. Has HitPoints
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties

        /// <summary>
        /// Object ignores damage or not
        /// </summary>
        [SerializeField] protected bool m_indestrutible;
        public bool IsIndestructible => m_indestrutible;

        /// <summary>
        /// Start value for HitPoints
        /// </summary>
        [SerializeField] protected int m_hitPoints;
        public int MaxHitPoints => m_hitPoints;

        /// <summary>
        /// Current value of HitPoints
        /// </summary>
        private int m_currentHitPoins;
        public int HitPoints => m_currentHitPoins;

        /// <summary>
        /// Event for object destruction
        /// </summary>
        [SerializeField] protected UnityEvent m_eventOnDeath;
        public UnityEvent EventOnDeath => m_eventOnDeath;

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_teamId;
        public int TeamId => m_teamId;

        private bool isDead = false;

        #endregion

        #region UnityEvents

        protected virtual void Start()
        {
            m_currentHitPoins = m_hitPoints;
        }

        #endregion

        #region PublicAPI
        /// <summary>
        /// Applying Damage to object
        /// </summary>
        /// <param name="damage">Damage to object</param>
        public void ApplyDamage(object sender, int damage)
        {
            if (m_indestrutible || isDead) return;

            m_currentHitPoins -= damage;

            if (m_currentHitPoins <= 0)
            {
                isDead = true;
                OnDeath();
            }
        }
        /// <summary>
        /// Applying Heal to object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="amount">Amount to heal</param>
        public void ApplyHeal(object sender, int amount)
        {
            if (m_currentHitPoins == m_hitPoints) return;

            m_currentHitPoins += amount;

            if (m_currentHitPoins > m_hitPoints)
                m_currentHitPoins = m_hitPoints;
        }
        /// <summary>
        /// Full restoration of hitpoints
        /// </summary>
        public void FullHeal() => m_currentHitPoins = m_hitPoints;

        #endregion

        /// <summary>
        /// Overdetermined event for object destruction, when HitPoints equal or less than 0
        /// </summary>
        protected virtual void OnDeath()
        {
            m_eventOnDeath?.Invoke();

            Destroy(gameObject);
        }

        private static HashSet<Destructible> m_allDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_allDestructibles;

        protected virtual void OnEnable()
        {
            if (m_allDestructibles == null)
                m_allDestructibles = new HashSet<Destructible>();

            m_allDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_allDestructibles.Remove(this);
        }

        public void SetTeam(int teamId)
        {
            m_teamId = teamId;
        }
    }
}
