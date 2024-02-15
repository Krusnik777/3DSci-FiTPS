using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public class QuestCollector : MonoBehaviour
    {
        [SerializeField] private Quest m_currentQuest;
        public Quest CurrentQuest => m_currentQuest;

        public UnityAction<Quest> EventOnQuestReceived;
        public UnityAction<Quest> EventOnQuestCompleted;
        public UnityAction EventOnLastQuestCompleted;

        public void AssignQuest(Quest quest)
        {
            m_currentQuest = quest;

            EventOnQuestReceived?.Invoke(m_currentQuest);

            m_currentQuest.OnQuestCompleted.AddListener(OnQuestCompleted);
        }

        private void Start()
        {
            if (m_currentQuest != null) AssignQuest(m_currentQuest);
        }

        private void OnQuestCompleted()
        {
            m_currentQuest.OnQuestCompleted.RemoveListener(OnQuestCompleted);

            EventOnQuestCompleted?.Invoke(m_currentQuest);

            if (m_currentQuest.NextQuest != null) AssignQuest(m_currentQuest.NextQuest);
            else EventOnLastQuestCompleted?.Invoke();
        }
    }
}
