using UnityEngine;

namespace SciFiTPS
{
    public class LookAt : MonoBehaviour
    {
        [SerializeField] private Transform m_target;

        private void Update()
        {
            transform.LookAt(m_target);
        }
    }
}
