using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    public abstract class EntityActionProperties { }

    public abstract class EntityAction : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_eventOnStart;
        [SerializeField] private UnityEvent m_eventOnEnd;

        public UnityEvent EventOnStart => m_eventOnStart;
        public UnityEvent EventOnEnd => m_eventOnEnd;

        private EntityActionProperties m_properties;
        public EntityActionProperties Properties => m_properties;

        private bool m_isStarted;

        public virtual void StartAction()
        {
            if (m_isStarted) return;

            m_isStarted = true;

            m_eventOnStart?.Invoke();
        }

        public virtual void EndAction()
        {
            m_isStarted = false;

            m_eventOnEnd?.Invoke();
        }

        public virtual void SetProperties(EntityActionProperties props)
        {
            m_properties = props;
        }
    }
}
