using UnityEngine;

namespace SciFiTPS
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_lifeTime = 1.0f;

        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= m_lifeTime)
            {
                Destroy(gameObject);
            }
        }

    }
}
