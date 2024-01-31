using UnityEngine;

namespace SciFiTPS
{
    [System.Serializable]
    public class WheelAxle
    {
        [SerializeField] private WheelCollider m_leftWheelCollider;
        [SerializeField] private WheelCollider m_rightWheelCollider;
        [SerializeField] private Transform m_leftWheelMesh;
        [SerializeField] private Transform m_rightWheelMesh;
        [SerializeField] private bool m_motor;
        [SerializeField] private bool m_steering;
        
        public bool Motor => m_motor;
        public bool Steering => m_steering;

        public void SetTorque(float torque)
        {
            if (!m_motor) return;

            m_leftWheelCollider.motorTorque = torque;
            m_rightWheelCollider.motorTorque = torque;
        }

        public void Brake(float brakeTorque)
        {
            m_leftWheelCollider.brakeTorque = brakeTorque;
            m_rightWheelCollider.brakeTorque = brakeTorque;
        }

        public void SetSteerAngle(float angle)
        {
            if (!m_steering) return;

            m_leftWheelCollider.steerAngle = angle;
            m_rightWheelCollider.steerAngle = angle;
        }

        public void UpdateMeshTransform()
        {
            UpdateWheelTransform(m_leftWheelCollider, ref m_leftWheelMesh);
            UpdateWheelTransform(m_rightWheelCollider, ref m_rightWheelMesh);
        }

        private void UpdateWheelTransform(WheelCollider wheelCollider, ref Transform wheelTransform)
        {
            //Vector3 position;
            //Quaternion rotation;

            wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheelTransform.position = position;
            wheelTransform.rotation = rotation;
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class WheeledVehicle : Vehicle
    {
        [SerializeField] private WheelAxle[] m_wheelAxles;
        [SerializeField] private float m_maxMotorTorque;
        [SerializeField] private float m_brakeTorque;
        [SerializeField] private float m_maxSteerAngle;
        [SerializeField] private float m_controlFactor = 1f;

        private Rigidbody m_rigidBody;

        private Vector3 vehicleControl;

        public override float LinearVelocity => m_rigidBody.velocity.magnitude;

        protected override void Start()
        {
            base.Start();

            m_rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            vehicleControl = Vector3.MoveTowards(vehicleControl, m_targetInputControl, Time.fixedDeltaTime * m_controlFactor);

            float targetMotorTorque = m_maxMotorTorque * vehicleControl.z;
            float targetBrakeTorque = m_brakeTorque * vehicleControl.y;
            float targetSteerAngle = m_maxSteerAngle * vehicleControl.x;


            for (int i = 0; i < m_wheelAxles.Length; i++)
            {
                if (targetBrakeTorque == 0 && LinearVelocity < m_maxLinearSpeed)
                {
                    m_wheelAxles[i].Brake(0);
                    m_wheelAxles[i].SetTorque(targetMotorTorque);
                }

                if (LinearVelocity > m_maxLinearSpeed)
                {
                    m_wheelAxles[i].Brake(targetBrakeTorque * 0.2f);
                }
                else
                {
                    m_wheelAxles[i].Brake(targetBrakeTorque);
                }

                m_wheelAxles[i].SetSteerAngle(targetSteerAngle);

                m_wheelAxles[i].UpdateMeshTransform();
            }
        }


    }
}
