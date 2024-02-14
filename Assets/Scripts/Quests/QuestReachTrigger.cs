using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(BoxCollider))]
    public class QuestReachTrigger : Quest
    {
        [SerializeField] private GameObject m_owner;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != m_owner) return;

            EventOnCompleted?.Invoke();
        }
    }
}
