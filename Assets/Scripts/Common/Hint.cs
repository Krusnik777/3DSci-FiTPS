using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(Canvas))]
    public class Hint : MonoBehaviour
    {
        [SerializeField] private GameObject m_hint;
        [SerializeField] private float m_activeRadius;

        private Canvas m_canvas;
        private Transform m_target;
        private Transform m_lookTransform;

        private void Start()
        {
            FindTarget();
        }

        private void Update()
        {
            if (m_target == null || m_lookTransform == null)
            {
                FindTarget();

                if (m_target == null || m_lookTransform == null) return;
            }

            m_hint.transform.LookAt(m_lookTransform);

            if (Vector3.Distance(transform.position, m_target.position) < m_activeRadius)
            {
                m_hint.SetActive(true);
            }
            else m_hint.SetActive(false);
        }

        private void FindTarget()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvas.worldCamera = Camera.main;
            m_lookTransform = Camera.main?.transform;
            m_target = Player.Instance.transform;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_activeRadius);
        }
        #endif

    }
}
