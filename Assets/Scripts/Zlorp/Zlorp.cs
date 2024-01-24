using UnityEngine;

namespace SciFiTPS
{
    public class Zlorp : Destructible
    {
        protected override void OnDeath()
        {
            m_eventOnDeath?.Invoke();
        }
    }
}
