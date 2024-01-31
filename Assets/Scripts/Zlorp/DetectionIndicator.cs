using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class DetectionIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject m_indicator;
        [SerializeField] private Text m_text;
        [SerializeField] private Image m_image;

        private Canvas m_canvas;
        private Transform m_lookTransform;

        public bool IsShowing => m_indicator.activeInHierarchy;

        public void Show(string symbols, Color color, float fillAmount)
        {
            m_text.color = color;
            m_text.text = symbols;
            m_image.color = color;
            m_image.fillAmount = fillAmount;

            m_indicator.SetActive(true);
        }

        public void Hide()
        {
            m_indicator.SetActive(false);
        }

        public void ShowAndHide(string symbols, Color color)
        {
            CancelInvoke(nameof(Hide));

            m_text.color = color;
            m_text.text = symbols;
            m_image.color = color;
            m_image.fillAmount = 1.0f;

            m_indicator.SetActive(true);

            Invoke(nameof(Hide), 1f);
        }

        private void Start()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvas.worldCamera = Camera.main;
            m_lookTransform = Camera.main.transform;

            m_indicator.SetActive(false);
        }

        private void Update()
        {
            m_indicator.transform.LookAt(m_lookTransform);
        }
    }
}
