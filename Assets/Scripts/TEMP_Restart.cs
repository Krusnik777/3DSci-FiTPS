using UnityEngine.SceneManagement;
using UnityEngine;

namespace SciFiTPS
{
    public class TEMP_Restart : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnTriggerEnter(Collider other)
        {
            RestartLevel();
        }
    }
}
