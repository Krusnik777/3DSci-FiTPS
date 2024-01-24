using UnityEngine;

namespace SciFiTPS
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private PatrolPathNode[] m_nodes;

        [ContextMenu("Update Path Node")]
        private void UpdatePathNode()
        {
            m_nodes = new PatrolPathNode[transform.childCount];

            for (int i = 0; i < m_nodes.Length; i++)
            {
                m_nodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
            }
        }

        public PatrolPathNode GetRandomPathNode() => m_nodes[Random.Range(0, m_nodes.Length)];

        public PatrolPathNode GetNextNode(ref int index)
        {
            index = Mathf.Clamp(index, 0, m_nodes.Length - 1);
            index++;

            if (index >= m_nodes.Length) index = 0;

            return m_nodes[index];
        }

        private void Start()
        {
            UpdatePathNode();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_nodes == null) return;

            Gizmos.color = Color.red;
            for (int i = 0; i < m_nodes.Length - 1; i++)
            {
                Gizmos.DrawLine(m_nodes[i].transform.position + new Vector3(0, 0.5f, 0), m_nodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
            }

            Gizmos.DrawLine(m_nodes[0].transform.position + new Vector3(0, 0.5f, 0), m_nodes[m_nodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
            
        }
        #endif
    }
}
