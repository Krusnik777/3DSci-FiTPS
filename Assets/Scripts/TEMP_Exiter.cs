using UnityEngine.SceneManagement;
using UnityEngine;

namespace SciFiTPS
{
    public class TEMP_Exiter : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKey(KeyCode.F1))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

}
