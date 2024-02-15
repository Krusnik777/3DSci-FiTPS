using UnityEngine;

namespace SciFiTPS
{
    [CreateAssetMenu]
    public class QuestProperties : ScriptableObject
    {
        [SerializeField][TextArea(1, 3)] private string m_description;
        [SerializeField][TextArea(1, 3)] private string m_task;

        public string Description => m_description;
        public string Task => m_task;
    }
}
