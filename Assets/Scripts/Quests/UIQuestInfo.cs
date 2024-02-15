using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class UIQuestInfo : MonoBehaviour
    {
        [SerializeField] private QuestCollector m_questCollector;
        [SerializeField] private Text m_description;
        [SerializeField] private Text m_task;

        private void Awake()
        {
            m_description.gameObject.SetActive(false);
            if (m_task != null) m_task.gameObject.SetActive(false);

            m_questCollector.EventOnQuestReceived += OnQuestReceived;
            m_questCollector.EventOnQuestCompleted += OnQuestCompleted;
        }

        private void OnDestroy()
        {
            m_questCollector.EventOnQuestReceived -= OnQuestReceived;
            m_questCollector.EventOnQuestCompleted -= OnQuestCompleted;
        }

        private void OnQuestReceived(Quest quest)
        {
            m_description.gameObject.SetActive(true);
            m_description.text = quest.Properties.Description;

            if (m_task != null)
            {
                m_task.gameObject.SetActive(true);

                if (quest is QuestKillDestructibles)
                {
                    var defeatQuest = quest as QuestKillDestructibles;

                    m_task.text = quest.Properties.Task + ": " + defeatQuest.AmountKilled.ToString() + "/" + defeatQuest.TargetAmount.ToString();

                    defeatQuest.EventOnDestructibleDead += UpdateQuest;
                }
                else m_task.text = quest.Properties.Task;
            }
        }

        private void OnQuestCompleted(Quest quest)
        {
            if (quest is QuestKillDestructibles)
            {
                var defeatQuest = quest as QuestKillDestructibles;

                defeatQuest.EventOnDestructibleDead -= UpdateQuest;
            }

            m_description.gameObject.SetActive(false);
            if (m_task != null) m_task.gameObject.SetActive(false);
        }

        private void UpdateQuest(Quest quest)
        {
            if (quest is QuestKillDestructibles)
            {
                var defeatQuest = quest as QuestKillDestructibles;

                m_task.text = quest.Properties.Task + ": " + defeatQuest.AmountKilled.ToString() + "/" + defeatQuest.TargetAmount.ToString();
            }
        }
    }
}
