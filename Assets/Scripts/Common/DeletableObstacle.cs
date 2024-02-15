using UnityEngine;

namespace SciFiTPS
{
    public class DeletableObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject m_explosionPrefab;

        public void DeleteObstacle()
        {
            var explosion = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);

            Destroy(explosion, 2f);

            Destroy(gameObject);
        }
    }
}
