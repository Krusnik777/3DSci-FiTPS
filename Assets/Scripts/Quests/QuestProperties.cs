using UnityEngine;

namespace SciFiTPS
{
    [CreateAssetMenu]
    public class QuestProperties : ScriptableObject
    {
        [SerializeField] private QuestType m_type;
        [SerializeField] private bool m_firstInLine;
        [SerializeField][TextArea(1, 3)] private string m_description;
        [SerializeField][TextArea(1, 3)] private string m_task;

        public bool FirstInLine => m_firstInLine;
        public QuestType Type => m_type;
        public string Description => m_description;
        public string Task => m_task;
    }
}
