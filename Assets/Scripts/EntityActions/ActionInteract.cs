using System.Collections;
using UnityEngine;

namespace SciFiTPS
{
    public enum InteractType
    {
        PickupItem,
        EnteringCode
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

            //base.StartAction();

            //m_owner.position = Properties.InteractTransform.position;
            StartCoroutine(MoveToInteractPosition());
        }

        public override void EndAction()
        {
            base.EndAction();

            CharacterMovement movement = m_owner.GetComponent<CharacterMovement>();
            movement.SetInteracting(false);
        }

        private IEnumerator MoveToInteractPosition()
        {
            CharacterMovement movement = m_owner.GetComponent<CharacterMovement>();
            movement.SetInteracting(true);

            if (movement.IsSprinting) movement.UnSprint();

            var targetPosition = Properties.InteractTransform.position;

            var elapsed = 0.0f;

            while (Vector3.Distance(m_owner.position, targetPosition) >= 0.5f)
            {
                float forwardFactor = Mathf.Lerp(0.0f, 1.0f, elapsed / 1.0f);

                movement.SetTargetDirection(targetPosition, forwardFactor);

                elapsed += Time.deltaTime;

                yield return null;
            }

            movement.ResetTargetDirection(targetPosition);

            base.StartAction();
        }
    }
}
