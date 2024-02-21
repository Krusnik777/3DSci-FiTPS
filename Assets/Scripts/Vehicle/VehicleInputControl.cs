using UnityEngine;

namespace SciFiTPS
{
    public class VehicleInputControl : MonoBehaviour
    {
        [SerializeField] private Vehicle m_vehicle;
        [SerializeField] private ThirdPersonCamera m_camera;
        [SerializeField] private Vector3 m_cameraOffset;
        [Header("Camera Rotation Limits")]
        [SerializeField] private float m_minLimitY;
        [SerializeField] private float m_maxLimitY;

        public virtual void AssignCamera(ThirdPersonCamera camera)
        { 
            m_camera = camera;
            m_camera.IsRotateTarget = false;
            m_camera.SetTargetOffset(m_cameraOffset);
            m_camera.SetTarget(m_vehicle.transform);
            m_camera.SetRotationLimit(m_minLimitY, m_maxLimitY);
        }

        public void Stop()
        {
            m_vehicle.SetTargetControl(new Vector3(0,9999,0));
            enabled = false;
        }

        protected virtual void Start()
        {
            if (m_camera != null)
            {
                m_camera.IsRotateTarget = false;
                m_camera.SetTargetOffset(m_cameraOffset);
            }
        }

        protected virtual void Update()
        {
            m_vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")));
            m_camera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }


    }
}
