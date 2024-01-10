using UnityEngine;
using UnityEngine.UI;

namespace SciFiTPS
{
    public class UISight : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_characterMovement;

        [SerializeField] private Image m_imageSight;

        private void Update()
        {
            m_imageSight.enabled = m_characterMovement.IsAiming;
        }
    }
}
