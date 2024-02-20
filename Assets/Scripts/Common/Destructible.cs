using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    /// <summary>
    /// Destructible object on scene. Has HitPoints
    /// </summary>
    public class Destructible : Entity, ISerializableEntity
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

        [SerializeField] private UnityEvent m_eventOnGetDamage;
        public UnityAction<Destructible> EventOnDamaged;

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_teamId;
        public int TeamId => m_teamId;

        private bool isDead = false;
        public bool IsDead => isDead;

        #endregion

        #region UnityEvents

        protected virtual void Start()
        {
            m_currentHitPoins = m_hitPoints;
        }

        #endregion

        #region PublicAPI

        public void SetHitPoints(int hitPoints)
        {
            m_currentHitPoins = Mathf.Clamp(hitPoints, 0, m_hitPoints);
        }

        /// <summary>
        /// Applying Damage to object
        /// </summary>
        /// <param name="damage">Damage to object</param>
        public void ApplyDamage(object sender, int damage)
        {
            if (m_indestrutible || isDead) return;

            m_currentHitPoins -= damage;

            if (sender is Projectile)
            {
                var projectile = sender as Projectile;
                EventOnDamaged?.Invoke(projectile.Parent);
            }

            m_eventOnGetDamage?.Invoke();

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

        public static Destructible FindNearest(Vector3 position)
        {
            float minDist = float.MaxValue;
            Destructible target = null;

            foreach (var dest in m_allDestructibles)
            {
                float curDist = Vector3.Distance(dest.transform.position, position);

                if (curDist < minDist)
                {
                    minDist = curDist;
                    target = dest;
                }
            }

            return target;
        }

        public static Destructible FindNearestNonTeamMember(Destructible destructible)
        {
            float minDist = float.MaxValue;
            Destructible target = null;

            foreach (var dest in m_allDestructibles)
            {
                float curDist = Vector3.Distance(dest.transform.position, destructible.transform.position);

                if (curDist < minDist && dest.TeamId != destructible.TeamId && !dest.IsDead)
                {
                    minDist = curDist;
                    target = dest;
                }
            }

            return target;
        }

        public static List<Destructible> GetAllTeamMembers(int teamId)
        {
            List<Destructible> teammates = new List<Destructible>();

            foreach (var dest in m_allDestructibles)
            {
                if (dest.TeamId == teamId) teammates.Add(dest);
            }

            return teammates;
        }

        public static List<Destructible> GetAllNonTeamMembers(int teamId)
        {
            List<Destructible> nonTeammates = new List<Destructible>();

            foreach (var dest in m_allDestructibles)
            {
                if (dest.TeamId != teamId) nonTeammates.Add(dest);
            }

            return nonTeammates;
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

        // Serialize
        [System.Serializable]
        public class State
        {
            public Vector3 Position;
            public int HitPoints;

            public State() { }
        }

        [SerializeField] private int m_entityId;
        public long EntityId => m_entityId;

        public virtual bool IsSerializable()
        {
            return m_currentHitPoins > 0;
        }

        public virtual string SerializeState()
        {
            State s = new State();

            s.Position = transform.position;
            s.HitPoints = m_currentHitPoins;

            return JsonUtility.ToJson(s);
        }

        public virtual void DeserializeState(string state)
        {
            State s = JsonUtility.FromJson<State>(state);

            transform.position = s.Position;
            m_currentHitPoins = s.HitPoints;
        }
    }
}
