using UnityEngine;

namespace SciFiTPS
{
    public class VehicleUseTrigger : TriggerInteractAction
    {
        [SerializeField] private ActionUseVehicleProperties m_useVehicleProperties;

        protected override void InitActionProperties()
        {
            m_action.SetProperties(m_useVehicleProperties);
        }
    }
}
