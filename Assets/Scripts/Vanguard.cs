using UnityEngine;

namespace SciFiTPS
{
    public class Vanguard : Destructible
    {
        protected override void OnDeath()
        {
            m_eventOnDeath?.Invoke();
        }
    }
}
