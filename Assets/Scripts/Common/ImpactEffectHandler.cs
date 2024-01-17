using UnityEngine;

namespace SciFiTPS
{
    public class ImpactEffectHandler : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private ImpactEffect m_defaultImpactEffectPrefab;
        [SerializeField] private ImpactEffect m_stoneImpactEffectPrefab;
        [SerializeField] private ImpactEffect m_metalImpactEffectPrefab;
        [SerializeField] private ImpactEffect m_fleshImpactEffectPrefab;
        [Header("PhysicMaterial")]
        [SerializeField] private PhysicMaterial m_stoneMaterial;
        [SerializeField] private PhysicMaterial m_metalMaterial;
        [SerializeField] private PhysicMaterial m_fleshMaterial;

        public ImpactEffect GetImpactPrefab(Collider collider)
        {
            if (collider is CharacterController) return m_fleshImpactEffectPrefab;

            PhysicMaterial physicMaterial = collider.material;

            if (physicMaterial.name == m_stoneMaterial.name + " (Instance)") return m_stoneImpactEffectPrefab;
            if (physicMaterial.name == m_metalMaterial.name + " (Instance)") return m_metalImpactEffectPrefab;
            if (physicMaterial.name == m_fleshMaterial.name + " (Instance)") return m_fleshImpactEffectPrefab;

            return m_defaultImpactEffectPrefab;
        }
    }
}
