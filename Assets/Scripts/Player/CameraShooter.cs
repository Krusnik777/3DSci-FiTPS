using UnityEngine;

namespace SciFiTPS
{
    public class CameraShooter : MonoBehaviour
    {
        [SerializeField] private Weapon m_weapon;
        [SerializeField] private Camera m_camera;
        [SerializeField] private RectTransform m_imageSigh;
        public Weapon Weapon => m_weapon;
        public Camera Camera => m_camera;

        public void Shoot()
        {
            RaycastHit hit;
            Ray ray = m_camera.ScreenPointToRay(m_imageSigh.position);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                m_weapon.FirePointLookAt(hit.point);
            }
            else
            {
                m_weapon.FirePointLookAt(m_camera.transform.position + ray.direction * 1000);
            }

            if (m_weapon.CanFire)
            {
                m_weapon.Fire();
            }
        }
    }
}
