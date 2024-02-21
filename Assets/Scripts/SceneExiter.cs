using UnityEngine.SceneManagement;
using UnityEngine;

namespace SciFiTPS
{
    public class SceneExiter : MonoBehaviour
    {
        private SceneSerializer sceneSerializer;

        public void EndGame()
        {
            sceneSerializer.DeleteSave();

            Application.Quit();
        }

        public void RestartLevel()
        {
            sceneSerializer.DeleteSave();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Start()
        {
            sceneSerializer = GetComponent<SceneSerializer>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                EndGame();
            }

            if (Input.GetKey(KeyCode.F1))
            {
                RestartLevel();
            }
        }
    }

}
