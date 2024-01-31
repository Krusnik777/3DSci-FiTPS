using UnityEngine;

namespace SciFiTPS
{
    public class ShootingVehicleInputControl : VehicleInputControl
    {
        [SerializeField] private CameraShooter m_cameraShooter;
        [SerializeField] private Transform m_aimPoint;

        protected override void Update()
        {
            base.Update();

            m_aimPoint.position = m_cameraShooter.Camera.transform.position + m_cameraShooter.Camera.transform.forward * 30;

            if (Input.GetButton("Fire1"))
            {
                m_cameraShooter.Shoot();
            }
        }
    }
}
