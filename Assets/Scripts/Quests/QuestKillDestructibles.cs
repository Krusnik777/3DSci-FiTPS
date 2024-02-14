using UnityEngine;

namespace SciFiTPS
{
    public class QuestKillDestructibles : Quest
    {
        [SerializeField] private Destructible[] m_destructibles;

        private int amountKilled = 0;

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

            if (amountKilled == m_destructibles.Length)
            {
                for (int i = 0; i < m_destructibles.Length; i++)
                {
                    if (m_destructibles[i] != null)
                        m_destructibles[i].EventOnDeath.RemoveListener(OnDestructibleDead);
                }

                EventOnCompleted?.Invoke();
            }
        }
    }
}
