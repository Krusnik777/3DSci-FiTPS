using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public class QuestKillDestructibles : Quest
    {
        [SerializeField] private List<Destructible> m_destructibles;

        public int TargetAmount => m_destructibles.Count;

        private int amountKilled = 0;
        public int AmountKilled => amountKilled;

        public UnityAction<Quest> EventOnDestructibleDead;

        public void ClearTargetsList()
        {
            m_destructibles.Clear();
            amountKilled = 0;
        }
        public void AssignTarget(Destructible target)
        {
            m_destructibles.Add(target);
            target.EventOnDeath.AddListener(OnDestructibleDead);
        }

        private void Start()
        {
            if (m_destructibles == null) return;

            for(int i = 0; i < m_destructibles.Count; i++)
            {
                m_destructibles[i].AssignKillQuest(this);
                m_destructibles[i].EventOnDeath.AddListener(OnDestructibleDead);
            }
        }

        private void OnDestructibleDead()
        {
            amountKilled++;

            EventOnDestructibleDead?.Invoke(this);

            if (amountKilled == m_destructibles.Count)
            {
                for (int i = 0; i < m_destructibles.Count; i++)
                {
                    if (m_destructibles[i] != null)
                        m_destructibles[i].EventOnDeath.RemoveListener(OnDestructibleDead);
                }

                isCleared = true;
                OnQuestCompleted?.Invoke();
            }
        }
    }
}
