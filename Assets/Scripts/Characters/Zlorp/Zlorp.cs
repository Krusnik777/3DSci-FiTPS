using UnityEngine;

namespace SciFiTPS
{
    public class Zlorp : Destructible, ISoundListener
    {
        [SerializeField] private Weapon m_weapon;
        [SerializeField] private SpreadShootRig m_spreadShootRig;
        [SerializeField] private AIZlorp m_zlorpAI;
        [SerializeField] private float m_hearingDistance = 30f;

        public void Fire(Vector3 target)
        {
            if (!m_weapon.CanFire) return;

            m_weapon.FirePointLookAt(target);
            m_weapon.Fire();
            m_spreadShootRig.Spread();
        }

        public void Heard(float distance)
        {
            if (distance <= m_hearingDistance)
            {
                m_zlorpAI.OnHeard();
            }
        }

        protected override void OnDeath()
        {
            m_eventOnDeath?.Invoke();
        }

        // Serialize
        [System.Serializable]
        public class AIZlorpState
        {
            public Vector3 Position;
            public int HitPoints;
            public int Behaviour;
            public int PatrolPathIndex;

            public AIZlorpState() { }
        }

        public override string SerializeState()
        {
            AIZlorpState s = new AIZlorpState();

            s.Position = transform.position;
            s.HitPoints = HitPoints;
            s.Behaviour = (int) m_zlorpAI.Behaviour;
            s.PatrolPathIndex = m_zlorpAI.PatrolPathIndex;

            return JsonUtility.ToJson(s);
        }

        public override void DeserializeState(string state)
        {
            AIZlorpState s = JsonUtility.FromJson<AIZlorpState>(state);

            m_zlorpAI.SetPosition(s.Position);
            SetHitPoints(s.HitPoints);
            m_zlorpAI.Behaviour = (AIZlorp.AIBehaviour) s.Behaviour;
            m_zlorpAI.PatrolPathIndex = s.PatrolPathIndex;
        }
    }
}
