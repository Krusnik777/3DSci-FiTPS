using UnityEngine;

namespace SciFiTPS
{
    public class CharacterAnimationState : MonoBehaviour
    {
        [SerializeField] private CharacterController m_targetCharacterController;
        [SerializeField] private Animator m_targetAnimator;
        [SerializeField] private CharacterMovement m_targetCharacterMovement;

        private void Update()
        {
            var movementSpeed = transform.InverseTransformDirection(m_targetCharacterController.velocity);

            m_targetAnimator.SetFloat("NormalizeMovementX", movementSpeed.x / m_targetCharacterMovement.GetCurrentSpeedByState());
            m_targetAnimator.SetFloat("NormalizeMovementZ", movementSpeed.z / m_targetCharacterMovement.GetCurrentSpeedByState());

            m_targetAnimator.SetBool("IsSprinting", m_targetCharacterMovement.IsSprinting);
            m_targetAnimator.SetBool("IsCrouching", m_targetCharacterMovement.IsCrouching);
            m_targetAnimator.SetBool("IsAiming", m_targetCharacterMovement.IsAiming);
            m_targetAnimator.SetBool("IsGrounded", m_targetCharacterMovement.IsGrounded);

            if (!m_targetCharacterMovement.IsGrounded)
            {
                m_targetAnimator.SetFloat("Jump", movementSpeed.y);
            }

            m_targetAnimator.SetFloat("DistanceToGround", m_targetCharacterMovement.DistanceToGround);

            Vector3 groundSpeed = m_targetCharacterController.velocity;
            groundSpeed.y = 0;
            m_targetAnimator.SetFloat("GroundSpeed", groundSpeed.magnitude);

        }
    }
}
