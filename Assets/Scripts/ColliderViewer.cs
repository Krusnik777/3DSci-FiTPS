using UnityEngine;

namespace SciFiTPS
{
    public class ColliderViewer : MonoBehaviour
    {
        [SerializeField] private float m_viewingAngle;
        [SerializeField] private float m_viewingDistance;
        [SerializeField] private float m_viewHeight;

        public bool IsObjectVisible(GameObject target)
        {
            ColliderViewpoints viewPoints = target.GetComponent<ColliderViewpoints>();

            if (viewPoints == null) return false;

            return viewPoints.IsVisibleFromPoint(transform.position + new Vector3(0, m_viewHeight, 0), transform.forward, m_viewingAngle, m_viewingDistance);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, m_viewHeight, 0), transform.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, m_viewingAngle, 0, m_viewingDistance, 1);
        }
        #endif
    }
}
