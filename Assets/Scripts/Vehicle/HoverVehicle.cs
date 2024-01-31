using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(Rigidbody))]
    public class HoverVehicle : Vehicle
    {
        [SerializeField] private float m_thrustForward;
        [SerializeField] private float m_thustTorque;
        [SerializeField] private float m_dragLinear;
        [SerializeField] private float m_dragAngular;
        [SerializeField] private float m_hoverHeight;
        [SerializeField] private float m_hoverForce;
        [SerializeField] private Transform[] m_hoverJets;

        private Rigidbody m_rigidBody;

        private bool m_isGrounded;

        public bool ApplyJetForce(Transform transform)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, m_hoverHeight))
            {
                float s = (m_hoverHeight - hit.distance) / m_hoverHeight;
                Vector3 force = (s * m_hoverForce) * hit.normal;

                m_rigidBody.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);
                return true;
            }

            return false;
        }

        protected override void Start()
        {
            base.Start();

            m_rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            ComputeForce();
        }

        private void ComputeForce()
        {
            m_isGrounded = false;

            for (int i = 0; i < m_hoverJets.Length; i++)
            {
                if (ApplyJetForce(m_hoverJets[i]))
                    m_isGrounded = true;
            }

            if (m_isGrounded)
            {
                m_rigidBody.AddRelativeForce(Vector3.forward * m_thrustForward * m_targetInputControl.z);
                m_rigidBody.AddRelativeTorque(Vector3.up * m_thustTorque * m_targetInputControl.x);
            }

            // Linear Drag
            {
                float dragFactor = m_thrustForward / m_maxLinearSpeed;
                Vector3 dragForce = m_rigidBody.velocity * -dragFactor;

                if (m_isGrounded)
                {
                    m_rigidBody.AddForce(dragForce, ForceMode.Acceleration);
                }
            }

            // Angular Drag
            {
                Vector3 dragForce = -m_rigidBody.angularVelocity * m_dragAngular;
                m_rigidBody.AddTorque(dragForce, ForceMode.Acceleration);
            }
        }

    }
}
