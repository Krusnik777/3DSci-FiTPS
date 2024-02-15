using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public class Quest : MonoBehaviour
    {
        [SerializeField] private QuestProperties m_questProperties;
        [SerializeField] private Quest m_nextQuest;
        [SerializeField] private Transform m_reachedPoint;

        public QuestProperties Properties => m_questProperties;
        public Quest NextQuest => m_nextQuest;
        public Transform ReachedPoint => m_reachedPoint;

        public UnityEvent OnQuestCompleted;

        private void Update()
        {
            UpdateCompleteCondition();
        }

        protected virtual void UpdateCompleteCondition() { }
    }
}
