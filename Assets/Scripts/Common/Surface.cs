using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(Collider))]
    public class Surface : MonoBehaviour
    {
        [SerializeField] private ImpactType m_impactType;
        public ImpactType Type => m_impactType;

        [ContextMenu("AddToAllObject")]
        public void AddToAllObject()
        {
            Transform[] allTransforms = GameObject.FindObjectsOfType<Transform>();

            for (int i = 0; i < allTransforms.Length; i++)
            {
                if (allTransforms[i].GetComponent<Collider>() != null)
                {
                    if (allTransforms[i].GetComponent<Surface>() == null)
                    {
                        allTransforms[i].gameObject.AddComponent<Surface>();
                    }
                }
            }

        }
    }
}
