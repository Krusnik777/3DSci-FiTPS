using UnityEngine;

namespace SciFiTPS
{
    public enum ImpactType
    {
        NoDecal,
        Default
    }

    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_lifeTime = 1.0f;
        [SerializeField] private GameObject m_decal;

        private float timer;

        public void UpdateType(ImpactType type)
        {
            if (type == ImpactType.NoDecal)
            {
                m_decal.SetActive(false);
            }
        }

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
