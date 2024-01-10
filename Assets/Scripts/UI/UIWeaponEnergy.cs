using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class UIWeaponEnergy : MonoBehaviour
    {
        [SerializeField] private Weapon m_weapon;
        [SerializeField] private Slider m_slider;
        [SerializeField] private Image[] m_images;

        private void Start()
        {
            m_slider.maxValue = m_weapon.PrimaryMaxEnergy;
            m_slider.value = m_slider.maxValue;
        }

        private void Update()
        {
            m_slider.value = m_weapon.PrimaryEnergy;

            SetActiveImages(m_weapon.PrimaryEnergy != m_weapon.PrimaryMaxEnergy);
        }

        private void SetActiveImages(bool active)
        {
            for (int i = 0; i < m_images.Length; i++)
            {
                m_images[i].enabled = active;
            }
        }
    }
}
