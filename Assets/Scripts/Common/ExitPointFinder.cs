using UnityEngine;

namespace SciFiTPS
{
    public class ExitPointFinder : MonoBehaviour
    {
        [SerializeField] private ColliderChecker[] m_colliderCheckers;

        [ContextMenu("Update Exit Points")]
        private void UpdateExitPoints()
        {
            m_colliderCheckers = GetComponentsInChildren<ColliderChecker>();
        }

        public Transform FindExitPoint()
        {
            foreach (var colliderChecker in m_colliderCheckers)
            {
                if (!colliderChecker.CheckColliders())
                {
                    return colliderChecker.transform;
                }
            }

            return null;
        }

        private void Start()
        {
            if (m_colliderCheckers.Length == 0) UpdateExitPoints();
        }
    }
}
