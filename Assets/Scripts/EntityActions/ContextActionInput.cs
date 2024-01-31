using System.Collections.Generic;
using UnityEngine;

namespace SciFiTPS
{
    public class ContextActionInput : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private EntityActionCollector m_actionCollector;

        private void Update()
        {
            if (Input.GetButtonDown("Use"))
            {
                if (!m_characterMovement.IsAiming && !m_characterMovement.IsCrouching)
                {
                    List<EntityContextAction> actionList = m_actionCollector.GetActionList<EntityContextAction>();

                    for (int i = 0; i < actionList.Count; i++)
                    {
                        actionList[i].StartAction();
                        
                        actionList[i].EndAction();
                    }
                }
            }
        }
    }
}
