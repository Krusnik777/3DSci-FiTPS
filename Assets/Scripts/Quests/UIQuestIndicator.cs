using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class UIQuestIndicator : MonoBehaviour
    {
        [SerializeField] private QuestCollector m_questCollector;
        [SerializeField] private Camera m_camera;
        [SerializeField] private Image m_indicator;
        [Header("Borders")]
        [SerializeField] private float m_widthBorder = 30f;
        [SerializeField] private float m_heigthBorder = 30f;

        private Transform reachedPoint;

        private void Start()
        {
            m_indicator.gameObject.SetActive(false);

            m_questCollector.EventOnQuestReceived += OnQuestReceived;
            m_questCollector.EventOnLastQuestCompleted += OnLastQuestCompleted;
        }

        private void OnDestroy()
        {
            m_questCollector.EventOnQuestReceived -= OnQuestReceived;
            m_questCollector.EventOnLastQuestCompleted -= OnLastQuestCompleted;
        }

        private void Update()
        {
            if (reachedPoint == null) return;

            Vector3 pos = m_camera.WorldToScreenPoint(reachedPoint.position);

            if (pos.z > 0)
            {
                if (pos.x < 0) pos.x = 0 + m_widthBorder;
                if (pos.x > Screen.width) pos.x = Screen.width - m_widthBorder;

                if (pos.y < 0) pos.y = 0 + m_heigthBorder;
                if (pos.y > Screen.height) pos.y = Screen.height - m_heigthBorder;

                m_indicator.transform.position = pos;
            }
        }

        private void OnQuestReceived(Quest quest)
        {
            reachedPoint = quest.ReachedPoint;

            m_indicator.gameObject.SetActive(true);
        }

        private void OnLastQuestCompleted()
        {
            reachedPoint = null;

            m_indicator.gameObject.SetActive(false);
        }
    }
}
