using UnityEngine.SceneManagement;
using UnityEngine;

namespace SciFiTPS
{
    public class TEMP_Restart : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_owners;

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var owner in m_owners)
            {
                if (other.gameObject == owner)
                {
                    RestartLevel();
                }
            }
        }
    }
}
