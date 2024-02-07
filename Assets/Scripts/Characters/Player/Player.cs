using UnityEngine;

namespace SciFiTPS
{
    public class Player : MonoSingleton<Player>
    {
        [SerializeField] private UIPlayerNotification m_uIPlayerNotic;

        private int m_pursuersNumber;

        public void StartPursue()
        {
            m_pursuersNumber++;
            m_uIPlayerNotic.Show();
        }

        public void StopPursue()
        {
            m_pursuersNumber--;

            if (m_pursuersNumber == 0)
            {
                m_uIPlayerNotic.Hide();
            }
        }
    }
}
