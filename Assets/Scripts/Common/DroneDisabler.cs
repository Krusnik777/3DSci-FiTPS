using UnityEngine;

namespace SciFiTPS
{
    public class DroneDisabler : MonoBehaviour
    {
        [SerializeField] private Drone[] m_selectedDrones;

        public void DisableSelectedDrones()
        {
            foreach (var drone in m_selectedDrones)
            {
                drone.ApplyDamage(this, 9999);
            }
        }

        public void DisableAllActiveDrones()
        {
            Drone[] allActiveDrones = FindObjectsOfType<Drone>();

            foreach (var drone in allActiveDrones)
            {
                drone.ApplyDamage(this, 9999);
            }
        }
    }
}
