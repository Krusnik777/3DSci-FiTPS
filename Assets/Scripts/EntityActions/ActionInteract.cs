using UnityEngine;

namespace SciFiTPS
{
    public enum InteractType
    {
        PickupItem
    }

    [System.Serializable]
    public class ActionInteractProperties : EntityActionProperties
    {
        [SerializeField] private Transform m_interactTransform;
        public Transform InteractTransform => m_interactTransform;
    }

    public class ActionInteract : EntityContextAction
    {
        [SerializeField] private Transform m_owner;
        [SerializeField] private InteractType m_type;
        public InteractType Type => m_type;

        private new ActionInteractProperties Properties;

        public override void SetProperties(EntityActionProperties props)
        {
            Properties = (ActionInteractProperties) props;
        }

        public override void StartAction()
        {
            if (!IsCanStart) return;

            base.StartAction();

            m_owner.position = Properties.InteractTransform.position;

        }
    }
}
