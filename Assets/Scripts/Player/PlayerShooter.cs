using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private Weapon m_weapon;
        [SerializeField] private SpreadShootRig m_spreadShootRig;
        [SerializeField] private Camera m_camera;
        [SerializeField] private RectTransform m_imageSigh;

        public bool IsFired => m_spreadShootRig.IsFired;

        public void Shoot()
        {
            if (!m_characterMovement.IsAiming) return;

            RaycastHit hit;
            Ray ray = m_camera.ScreenPointToRay(m_imageSigh.position);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                m_weapon.FirePointLookAt(hit.point);
            }

            if (m_weapon.CanFire)
            {
                m_weapon.Fire();
                m_spreadShootRig.Spread();
            }
        }
    }
}
