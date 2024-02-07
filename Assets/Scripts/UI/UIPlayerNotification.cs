using UnityEngine;

namespace SciFiTPS
{
    public class UIPlayerNotification : MonoBehaviour
    {
        [SerializeField] private GameObject m_hit;

        public void Show()
        {
            m_hit.SetActive(true);
        }

        public void Hide()
        {
            m_hit.SetActive(false);
        }
    }
}
