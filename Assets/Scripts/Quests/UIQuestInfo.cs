using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class UIQuestInfo : MonoBehaviour
    {
        [SerializeField] private QuestCollector m_questCollector;
        [SerializeField] private Text m_description;
        [SerializeField] private Text m_task;

        private void Start()
        {
            m_description.gameObject.SetActive(false);
            m_task.gameObject.SetActive(false);

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
            m_task.gameObject.SetActive(true);

            m_description.text = quest.Properties.Description;
            m_task.text = quest.Properties.Task;
        }

        private void OnQuestCompleted(Quest quest)
        {
            m_description.gameObject.SetActive(false);
            m_task.gameObject.SetActive(false);
        }
    }
}
