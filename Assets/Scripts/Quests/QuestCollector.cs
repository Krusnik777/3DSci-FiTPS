using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public enum QuestType
    {
        Main,
        Current
    }

    public class QuestCollector : MonoBehaviour
    {
        [SerializeField] private QuestType m_questLineType;
        [SerializeField] private Quest m_currentQuest;

        public QuestType Type => m_questLineType;
        public Quest CurrentQuest => m_currentQuest;

        public UnityAction<Quest> EventOnQuestReceived;
        public UnityAction<Quest> EventOnQuestCompleted;
        public UnityAction EventOnLastQuestCompleted;

        public void AssignQuest(Quest quest)
        {
            m_currentQuest = quest;

            if (m_currentQuest == null) return;

            EventOnQuestReceived?.Invoke(m_currentQuest);

            m_currentQuest.OnQuestCompleted.AddListener(OnQuestCompleted);
        }

        private void Start()
        {
            if (m_currentQuest != null) AssignQuest(m_currentQuest);
            else
            {
                AssignQuest(FindActiveQuest());
            }
        }

        private void OnQuestCompleted()
        {
            m_currentQuest.OnQuestCompleted.RemoveListener(OnQuestCompleted);

            EventOnQuestCompleted?.Invoke(m_currentQuest);

            if (m_currentQuest.NextQuest != null) AssignQuest(m_currentQuest.NextQuest);
            else EventOnLastQuestCompleted?.Invoke();
        }

        private Quest FindActiveQuest()
        {
            Quest[] quests = FindObjectsOfType<Quest>();

            foreach (var quest in quests)
            {
                if (quest.Properties.Type == m_questLineType)
                {
                    if (quest.Properties.FirstInLine)
                    {
                        return quest.GetActiveQuestInLine();
                    }
                }
            }

            return null;
        }
    }
}
