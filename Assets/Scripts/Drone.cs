using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(DroneMovement))]
    public class Drone : Destructible
    {
        [Header("Main")]
        [SerializeField] private Transform m_mainMesh;

        [Header("View")]
        [SerializeField] private GameObject[] m_meshComponents;
        [SerializeField] private Renderer[] m_meshRenderers;
        [SerializeField] private Material[] m_deadMaterials;

        [Header("Movement")]
        [SerializeField] private DroneMovement m_droneMovenent;
        [SerializeField] private float m_hoverAmplitude;
        [SerializeField] private float m_hoverSpeed;

        private void Update()
        {
            m_mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * m_hoverAmplitude) * m_hoverSpeed * Time.deltaTime, 0);
        }

        protected override void OnDeath()
        {
            m_eventOnDeath?.Invoke();

            enabled = false;
            m_droneMovenent.enabled = false;

            for (int i = 0; i < m_meshComponents.Length; i++)
            {
                if (m_meshComponents[i].GetComponent<Rigidbody>() == null)
                    m_meshComponents[i].AddComponent<Rigidbody>();
            }

            for (int i = 0; i < m_meshRenderers.Length; i++)
            {
                m_meshRenderers[i].material = m_deadMaterials[i];
            }
        }
    }
}
