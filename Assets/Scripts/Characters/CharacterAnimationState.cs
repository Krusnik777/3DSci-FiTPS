using UnityEngine;

namespace SciFiTPS
{
    [System.Serializable]
    public class CharacterAnimatorParametersName
    {
        public string NormalizeMovementX;
        public string NormalizeMovementZ;
        public string Sprint;
        public string Crouch;
        public string Aiming;
        public string Ground;
        public string Jump;
        public string GroundSpeed;
        public string DistanceToGround;
    }

    [System.Serializable]
    public class AnimationCrossFadeParameters
    {
        public string Name;
        public float Duration;
    }

    public class CharacterAnimationState : MonoBehaviour
    {
        private const float INPUT_CONTROL_LERP_RATE = 10.0f;

        [SerializeField] private CharacterController m_targetCharacterController;
        [SerializeField] private CharacterMovement m_targetCharacterMovement;
        [Space(5)] [SerializeField] private CharacterAnimatorParametersName m_animatorParameterNames;
        [SerializeField] private Animator m_targetAnimator;

        [Header("Fades")] [Space(5)]
        [SerializeField] private AnimationCrossFadeParameters m_fallFade;
        [SerializeField] private float m_minDistanceToGroundByFall;
        [SerializeField] private AnimationCrossFadeParameters m_jumpIdleFade;
        [SerializeField] private AnimationCrossFadeParameters m_jumpMoveFade;

        private Vector3 inputControl;

        // TEMP
        private float groundedTimer = 0.2f;
        private float timer = 0;

        private void LateUpdate()
        {
            var movementSpeed = transform.InverseTransformDirection(m_targetCharacterController.velocity);

            inputControl = Vector3.MoveTowards(inputControl, m_targetCharacterMovement.TargetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

            m_targetAnimator.SetFloat(m_animatorParameterNames.NormalizeMovementX, inputControl.x);
            m_targetAnimator.SetFloat(m_animatorParameterNames.NormalizeMovementZ, inputControl.z);

            m_targetAnimator.SetBool(m_animatorParameterNames.Sprint, m_targetCharacterMovement.IsSprinting);
            m_targetAnimator.SetBool(m_animatorParameterNames.Crouch, m_targetCharacterMovement.IsCrouching);
            m_targetAnimator.SetBool(m_animatorParameterNames.Aiming, m_targetCharacterMovement.IsAiming);
            m_targetAnimator.SetBool(m_animatorParameterNames.Ground, m_targetCharacterMovement.IsGrounded);

            Vector3 groundSpeed = m_targetCharacterController.velocity;
            groundSpeed.y = 0;
            m_targetAnimator.SetFloat(m_animatorParameterNames.GroundSpeed, groundSpeed.magnitude);

            if (m_targetCharacterMovement.IsJumping)
            {
                if (groundSpeed.magnitude <= 0.01f)
                {
                    CrossFade(m_jumpIdleFade);
                }

                if (groundSpeed.magnitude > 0.01f)
                {
                    CrossFade(m_jumpMoveFade);
                }
            }

            if (!m_targetCharacterMovement.IsGrounded)
            {
                timer += Time.fixedDeltaTime;

                if (timer < groundedTimer) return;

                m_targetAnimator.SetFloat(m_animatorParameterNames.Jump, movementSpeed.y);

                if (movementSpeed.y < 0 && m_targetCharacterMovement.DistanceToGround > m_minDistanceToGroundByFall)
                {
                    CrossFade(m_fallFade);
                }
            }
            else
            {
                timer = 0;
            }

            m_targetAnimator.SetFloat(m_animatorParameterNames.DistanceToGround, m_targetCharacterMovement.DistanceToGround);
        }

        private void CrossFade(AnimationCrossFadeParameters parameters)
        {
            m_targetAnimator.CrossFade(parameters.Name, parameters.Duration);
        }
    }
}
