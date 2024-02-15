using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public class QuestKillDestructibles : Quest
    {
        [SerializeField] private Destructible[] m_destructibles;

        public int TargetAmount => m_destructibles.Length;

        private int amountKilled = 0;
        public int AmountKilled => amountKilled;

        public UnityAction<Quest> EventOnDestructibleDead;

        private void Start()
        {
            if (m_destructibles == null) return;

            for(int i = 0; i < m_destructibles.Length; i++)
            {
                m_destructibles[i].EventOnDeath.AddListener(OnDestructibleDead);
            }
        }

        private void OnDestructibleDead()
        {
            amountKilled++;

            EventOnDestructibleDead?.Invoke(this);

            if (amountKilled == m_destructibles.Length)
            {
                for (int i = 0; i < m_destructibles.Length; i++)
                {
                    if (m_destructibles[i] != null)
                        m_destructibles[i].EventOnDeath.RemoveListener(OnDestructibleDead);
                }

                OnQuestCompleted?.Invoke();
            }
        }
    }
}
