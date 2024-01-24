using UnityEngine;

namespace SciFiTPS
{
    public class PatrolPathNode : MonoBehaviour
    {
        [SerializeField] private float m_idleTime;
        public float IdleTime => m_idleTime;

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
        #endif
    }
}
