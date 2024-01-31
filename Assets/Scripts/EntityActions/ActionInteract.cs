using System.Collections;
using UnityEngine;

namespace SciFiTPS
{
    public enum InteractType
    {
        PickupItem,
        EnteringCode,
        ClimbLadder,
        UseVehicle
    }

    [System.Serializable]
    public class ActionInteractProperties : EntityActionProperties
    {
        [SerializeField] private Transform m_interactTransform;
        public Transform InteractTransform => m_interactTransform;
        [SerializeField] private Transform m_endPoint;
        public Transform EndPoint => m_endPoint;
        [SerializeField] private Transform m_finalPosition;
        public Transform FinalPosition => m_finalPosition;
    }

    public class ActionInteract : EntityContextAction
    {
        [SerializeField] protected Transform m_owner;
        [SerializeField] private InteractType m_type;
        [SerializeField] private UnityEngine.Animations.Rigging.Rig m_leftHandRig;
        [SerializeField] private bool m_hideWeapon;
        [SerializeField] private bool m_moveToInteractPoint = true;
        [SerializeField] private GameObject m_weaponMeshGameObject;
        public InteractType Type => m_type;

        protected new ActionInteractProperties Properties;

        public override void SetProperties(EntityActionProperties props)
        {
            Properties = (ActionInteractProperties) props;
        }

        public override void StartAction()
        {
            if (!IsCanStart) return;

            m_leftHandRig.weight = 0;

            //base.StartAction();

            //m_owner.position = Properties.InteractTransform.position;

            if (m_moveToInteractPoint)
                StartCoroutine(MoveToInteractPosition());
            else base.StartAction();

            Invoke("SetCanEnd", 0.1f);
        }

        public override void EndAction()
        {
            if (!IsCanEnd) return;

            base.EndAction();

            m_leftHandRig.weight = 1;

            if (m_hideWeapon) m_weaponMeshGameObject.SetActive(true);

            CharacterMovement movement = m_owner.GetComponent<CharacterMovement>();
            movement.SetInteracting(false);
        }

        private void SetCanEnd()
        {
            IsCanEnd = true;
        }

        #region Coroutines

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

            if (m_hideWeapon) m_weaponMeshGameObject.SetActive(false);

            base.StartAction();
        }

        #endregion
    }
}
