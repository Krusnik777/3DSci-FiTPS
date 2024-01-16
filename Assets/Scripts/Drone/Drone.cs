using UnityEngine;

namespace SciFiTPS
{
    public class Drone : Destructible
    {
        [Header("Main")]
        [SerializeField] private Transform m_mainMesh;
        [SerializeField] private Weapon[] m_turrets;

        [Header("View")]
        [SerializeField] private GameObject[] m_meshComponents;
        [SerializeField] private Renderer[] m_meshRenderers;
        [SerializeField] private Material[] m_deadMaterials;

        [Header("Movement")]
        [SerializeField] private float m_movementSpeed = 2f;
        [SerializeField] private float m_rotationLerpFactor = 100f;
        [SerializeField] private float m_hoverAmplitude;
        [SerializeField] private float m_hoverSpeed;

        public Transform MainMesh => m_mainMesh;

        public void LookAt(Vector3 target)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), m_rotationLerpFactor * Time.deltaTime);
        }

        public void MoveTo(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, m_movementSpeed * Time.deltaTime);
        }

        public void Fire(Vector3 target)
        {
            for (int i = 0; i < m_turrets.Length; i++)
            {
                m_turrets[i].FirePointLookAt(target);
                m_turrets[i].Fire();
            }
        }

        private void Update()
        {
            Hover();
        }

        protected override void OnDeath()
        {
            m_eventOnDeath?.Invoke();

            enabled = false;

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

        private void Hover()
        {
            m_mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * m_hoverAmplitude) * m_hoverSpeed * Time.deltaTime, 0);
        }
    }
}
