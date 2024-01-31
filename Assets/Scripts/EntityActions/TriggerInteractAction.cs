using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SciFiTPS
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerInteractAction : MonoBehaviour
    {
        [SerializeField] private InteractType m_interactType;
        [SerializeField] private int m_interactAmount;
        [SerializeField] private UnityEvent m_eventOnStartInteract;
        [SerializeField] private UnityEvent m_eventOnEndInteract;
        [SerializeField] protected ActionInteractProperties m_actionProperties;

        public UnityEvent EventOnStartInteract => m_eventOnStartInteract;
        public UnityEvent EventOnEndInteract => m_eventOnEndInteract;

        protected ActionInteract m_action;

        protected GameObject m_owner;

        private ActionInteract GetActionInteract(EntityActionCollector entityActionCollector)
        {
            List<ActionInteract> actions = entityActionCollector.GetActionList<ActionInteract>();

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].Type == m_interactType) return actions[i];
            }

            return null;
        }

        protected virtual void InitActionProperties()
        {
            m_action.SetProperties(m_actionProperties);
        }

        protected virtual void OnStartAction(GameObject owner) { }
        protected virtual void OnEndAction(GameObject owner) { }

        private void OnTriggerEnter(Collider other)
        {
            if (m_interactAmount == 0) return;

            EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

            if (actionCollector != null)
            {
                m_action = GetActionInteract(actionCollector);

                if (m_action != null)
                {
                    InitActionProperties();

                    m_action.IsCanStart = true;
                    m_action.EventOnStart.AddListener(OnActionStarted);
                    m_action.EventOnEnd.AddListener(OnActionEnded);
                    m_owner = other.gameObject;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (m_interactAmount == 0) return;

            EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

            if (actionCollector != null)
            {
                m_action = GetActionInteract(actionCollector);

                if (m_action != null)
                {
                    m_action.IsCanStart = false;
                    m_action.EventOnStart.RemoveListener(OnActionStarted);
                    m_action.EventOnEnd.RemoveListener(OnActionEnded);
                }
            }
        }

        private void OnActionStarted()
        {
            OnStartAction(m_owner);

            m_interactAmount--;

            m_eventOnStartInteract?.Invoke();
        }

        private void OnActionEnded()
        {
            m_action.IsCanStart = false;
            m_action.IsCanEnd = false;

            m_action.EventOnStart.RemoveListener(OnActionStarted);
            m_action.EventOnEnd.RemoveListener(OnActionEnded);

            m_eventOnEndInteract?.Invoke();

            OnEndAction(m_owner);
        }
    }
}
