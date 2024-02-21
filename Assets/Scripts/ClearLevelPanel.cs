using UnityEngine;

namespace SciFiTPS
{
    public class ClearLevelPanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_panel;
        [SerializeField] private CharacterInputController m_characterInputController;

        public void ActivatePanel()
        {
            m_panel.SetActive(true);

            if (!m_characterInputController.enabled) m_characterInputController.enabled = true;

            m_characterInputController.UnlockMouse();
        }
    }
}
