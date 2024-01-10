using UnityEngine;

namespace SciFiTPS
{
    public class ImpactEffectHandler : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private ImpactEffect m_defaultImpactEffectPrefab;
        [SerializeField] private ImpactEffect m_stoneImpactEffectPrefab;
        [SerializeField] private ImpactEffect m_metalImpactEffectPrefab;
        [Header("PhysicMaterial")]
        [SerializeField] private PhysicMaterial m_stoneMaterial;
        [SerializeField] private PhysicMaterial m_metalMaterial;

        public ImpactEffect GetImpactPrefabByMaterial(PhysicMaterial physicMaterial)
        {
            if (physicMaterial.name == m_stoneMaterial.name + " (Instance)") return m_stoneImpactEffectPrefab;
            if (physicMaterial.name == m_metalMaterial.name + " (Instance)") return m_metalImpactEffectPrefab;

            return m_defaultImpactEffectPrefab;
        }
    }
}
