using System.Collections.Generic;
using UnityEngine;

namespace SciFiTPS
{
    public class EntityActionCollector : MonoBehaviour
    {
        [SerializeField] private Transform m_parentTransformWithActions;

        private List<EntityAction> m_allActions = new List<EntityAction>();

        public T GetAction<T>() where T : EntityAction
        {
            for (int i = 0; i < m_allActions.Count; i++)
            {
                if (m_allActions[i] is T) return (T)m_allActions[i];
            }

            return null;
        }

        public List<T> GetActionList<T>() where T : EntityAction
        {
            List<T> actions = new List<T>();

            for (int i = 0; i < m_allActions.Count; i++)
            {
                if (m_allActions[i] is T) actions.Add((T)m_allActions[i]);
            }

            return actions;
        }

        private void Awake()
        {
            for(int i = 0; i < m_parentTransformWithActions.childCount; i++)
            {
                if (m_parentTransformWithActions.GetChild(i).gameObject.activeSelf)
                {
                    EntityAction action = m_parentTransformWithActions.GetChild(i).GetComponent<EntityAction>();
                    if (action != null) m_allActions.Add(action);
                }
            }
        }
    }
}
