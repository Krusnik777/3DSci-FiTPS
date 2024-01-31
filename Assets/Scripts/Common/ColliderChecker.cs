using UnityEngine;

namespace SciFiTPS
{
    public class ColliderChecker : MonoBehaviour
    {
        [SerializeField] private float m_checkRadius;
        public float Radius => m_checkRadius;

        public bool CheckColliders()
        {
            Vector3 pos = transform.position;
            pos.y = m_checkRadius + 0.1f;

            Collider[] rangeChecks = Physics.OverlapSphere(pos, m_checkRadius);

            for (int i = 0; i < rangeChecks.Length; i++)
            {
                Debug.Log(gameObject.name + " : " + rangeChecks.Length + " : " + rangeChecks[i].gameObject.name);
            }

            if (rangeChecks.Length != 0) return true;

            return false;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, m_checkRadius + 0.1f, 0), m_checkRadius);
        }
        #endif
    }
}
