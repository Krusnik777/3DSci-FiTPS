using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SciFiTPS
{
    public class CubeArea : MonoBehaviour
    {
        [SerializeField] private Vector3 m_area;

        public static CubeArea CreateCubeArea(Vector3 position, Vector3 areaSize)
        {
            GameObject gameObject = new GameObject("SearchArea");
            var area = gameObject.AddComponent<CubeArea>();
            area.SetAreaSize(areaSize);

            return area;
        }

        public void SetAreaSize(Vector3 area) => m_area = area;

        public Vector3 GetRandomInsideZone()
        {
            Vector3 result = transform.position;

            result.x += Random.Range(-m_area.x / 2, m_area.x / 2);
            result.y += Random.Range(-m_area.y / 2, m_area.y / 2);
            result.z += Random.Range(-m_area.z / 2, m_area.z / 2);

            return result;
        }

        #if UNITY_EDITOR

        private static Color gizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = gizmoColor;
            Handles.DrawWireCube(transform.position, m_area);
        }

        #endif
    }
}
