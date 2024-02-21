using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public class Quest : MonoBehaviour
    {
        [SerializeField] private QuestProperties m_questProperties;
        [SerializeField] private Quest m_nextQuest;
        [SerializeField] private Transform m_reachedPoint;
        [SerializeField] private bool m_saveAtCompleted;

        public QuestProperties Properties => m_questProperties;
        public Quest NextQuest => m_nextQuest;
        public Transform ReachedPoint => m_reachedPoint;

        public UnityEvent OnQuestCompleted;

        protected bool isCleared = false;
        public bool IsCleared => isCleared;

        public Quest GetActiveQuestInLine()
        {
            if (!isCleared) 
                return this;
            else
            {
                if (m_nextQuest != null)
                {
                    return m_nextQuest.GetActiveQuestInLine();
                }
            }

            return null;
        }

        private void Update()
        {
            UpdateCompleteCondition();
        }

        protected virtual void UpdateCompleteCondition() { }

        protected virtual void SaveOnClear(Collider other)
        {
            if (!m_saveAtCompleted) return;

            SceneSerializer sceneSerializer = other.transform.root.GetComponentInChildren<SceneSerializer>();

            if (sceneSerializer == null) return;

            sceneSerializer.SaveScene();
        }
    }
}
