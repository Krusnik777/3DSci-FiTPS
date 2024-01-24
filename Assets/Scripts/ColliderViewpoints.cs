using UnityEngine;

namespace SciFiTPS
{
    public class ColliderViewpoints : MonoBehaviour
    {
        private enum ColliderType
        {
            Character
        }

        [SerializeField] private ColliderType m_colliderType;
        [SerializeField] private Collider m_collider;
        
        private Vector3[] m_points;

        [ContextMenu("Update View Points")]
        private void UpdateViewPoints()
        {
            if (m_collider == null) return;

            m_points = null;

            if (m_colliderType == ColliderType.Character) UpdatePointsForCharacterController();
        }

        public bool IsVisibleFromPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
        {
            for (int i = 0; i < m_points.Length; i++)
            {
                float angle = Vector3.Angle(m_points[i] - point, eyeDir);
                float dist = Vector3.Distance(m_points[i], point);

                if (angle <= viewAngle * 0.5f && dist <= viewDistance)
                {
                    RaycastHit hit;

                    Debug.DrawLine(point, m_points[i], Color.blue);
                    if (Physics.Raycast(point, (m_points[i] - point).normalized, out hit, viewDistance * 2))
                    {
                        if (hit.collider == m_collider) return true;
                    }
                }
            }

            return false;
        }

        private void Start()
        {
            if (m_colliderType == ColliderType.Character) UpdatePointsForCharacterController();
        }

        private void Update()
        {
            if (m_colliderType == ColliderType.Character) CalculatePointsForCharacterController(m_collider as CharacterController);
        }

        private void UpdatePointsForCharacterController()
        {
            if (m_points == null) m_points = new Vector3[4];

            CharacterController collider = m_collider as CharacterController;

            CalculatePointsForCharacterController(collider);
        }

        private void CalculatePointsForCharacterController(CharacterController collider)
        {
            m_points[0] = collider.transform.position + collider.center + collider.transform.up * collider.height * 0.3f;
            m_points[1] = collider.transform.position + collider.center - collider.transform.up * collider.height * 0.3f;
            m_points[2] = collider.transform.position + collider.center + collider.transform.right * collider.radius * 0.4f;
            m_points[3] = collider.transform.position + collider.center - collider.transform.right * collider.radius * 0.4f;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_points == null) return;

            Gizmos.color = Color.blue;
            for (int i = 0; i < m_points.Length; i++) Gizmos.DrawSphere(m_points[i], 0.1f);
        }
        #endif
    }
}
