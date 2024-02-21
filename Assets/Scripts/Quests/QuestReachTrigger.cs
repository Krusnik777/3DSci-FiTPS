using System.Collections.Generic;
using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(BoxCollider))]
    public class QuestReachTrigger : Quest
    {
        [SerializeField] private List<GameObject> m_owners;
        [SerializeField] private bool m_completeOnlyByPlayer;

        public void ClearOwners() => m_owners.Clear();
        public void AssignOwner(GameObject owner)
        {
            if (m_completeOnlyByPlayer)
            {
                if (owner.GetComponent<Player>() != null)
                {
                    m_owners.Add(owner);
                }

                return;
            }

            m_owners.Add(owner);
        }

        public void CompleteLevel()
        {
            foreach (var owner in m_owners)
            {
                if (owner.TryGetComponent(out Vehicle vehicle))
                {
                    vehicle.GetComponent<VehicleInputControl>().Stop();
                }
            }

            Player.Instance.gameObject.GetComponentInChildren<ClearLevelPanel>().ActivatePanel();
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var owner in m_owners)
            {
                if (other.gameObject == owner)
                {
                    isCleared = true;
                    OnQuestCompleted?.Invoke();
                    SaveOnClear(other);
                    break;
                }
            }
        }


        
        
    }
}
