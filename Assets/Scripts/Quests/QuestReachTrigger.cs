using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(BoxCollider))]
    public class QuestReachTrigger : Quest
    {
        [SerializeField] private GameObject[] m_owners;

        private void OnTriggerEnter(Collider other)
        {
            foreach (var owner in m_owners)
            {
                if (other.gameObject == owner)
                {
                    OnQuestCompleted?.Invoke();
                }
            }
        }
    }
}
