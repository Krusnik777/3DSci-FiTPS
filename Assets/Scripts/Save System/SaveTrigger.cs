using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(BoxCollider))]
    public class SaveTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Player>() == null) return;

            SceneSerializer sceneSerializer = other.transform.root.GetComponentInChildren<SceneSerializer>();

            if (sceneSerializer == null) return;

            sceneSerializer.SaveScene();
        }
    }
}
