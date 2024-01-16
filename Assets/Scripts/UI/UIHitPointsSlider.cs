using UnityEngine;
using UnityEngine.UI;


namespace SciFiTPS
{
    public class UIHitPointsSlider : MonoBehaviour
    {
        [SerializeField] private Destructible m_destructible;
        [SerializeField] private Slider m_slider;

        private void Update()
        {
            m_slider.value = m_destructible.HitPoints;
        }
    }
}
