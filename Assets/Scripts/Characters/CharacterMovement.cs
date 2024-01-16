using UnityEngine;

namespace SciFiTPS
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController m_characterController;
        [Header("Movement")]
        [SerializeField] private float m_accelerationRate = 1.0f;
        [SerializeField] private float m_rifleRunSpeed;
        [SerializeField] private float m_rifleSprintSpeed;
        [SerializeField] private float m_aimingWalkSpeed;
        [SerializeField] private float m_aimingRunSpeed;
        [SerializeField] private float m_crouchMoveSpeed;
        [SerializeField] private float m_jumpSpeed;
        [Header("State")]
        [SerializeField] private float m_crouchHeight;
        public float CrounchHeight => m_crouchHeight;

        [HideInInspector] public Vector3 TargetDirectionControl;

        private bool isAiming;
        public bool IsAiming => isAiming;
        private bool isJumping;
        public bool IsJumping => isJumping;
        private bool isCrouching;
        public bool IsCrouching => isCrouching;
        private bool isSprinting;
        public bool IsSprinting => isSprinting;

        private float distanceToGround;
        public float DistanceToGround => distanceToGround;
        public bool IsGrounded => distanceToGround < 0.01f;

        private float baseCharacterHeight;
        private float baseCharacterHeightOffset;

        private Vector3 directionControl;
        private Vector3 movementDirection;

        public void Jump()
        {
            if (!IsGrounded || isCrouching) return;

            isJumping = true;
        }

        public void Crouch()
        {
            if (!IsGrounded || isSprinting) return;

            isCrouching = true;
            m_characterController.height = m_crouchHeight;
            m_characterController.center = new Vector3(0, baseCharacterHeightOffset / 2, 0);
        }

        public void UnCrouch()
        {
            isCrouching = false;

            m_characterController.height = baseCharacterHeight;
            m_characterController.center = new Vector3(0, baseCharacterHeightOffset, 0);
        }

        public void Sprint()
        {
            if (!IsGrounded || isCrouching) return;

            isSprinting = true;
        }

        public void UnSprint()
        {
            isSprinting = false;
        }

        public void Aim()
        {
            isAiming = true;
        }

        public void UnAim()
        {
            isAiming = false;
        }

        private void Start()
        {
            baseCharacterHeight = m_characterController.height;
            baseCharacterHeightOffset = m_characterController.center.y;
        }

        private void Update()
        {
            Move();

            UpdateDistanceToGround();
        }

        public float GetCurrentSpeedByState()
        {
            if (isCrouching) return m_crouchMoveSpeed;

            if (isAiming)
            {
                if (isSprinting) return m_aimingRunSpeed;
                else return m_aimingWalkSpeed;
            }

            if (isSprinting) return m_rifleSprintSpeed;

            return m_rifleRunSpeed;
        }

        private void Move()
        {
            directionControl = Vector3.MoveTowards(directionControl, TargetDirectionControl, Time.deltaTime * m_accelerationRate);

            if (IsGrounded)
            {
                movementDirection = directionControl * GetCurrentSpeedByState();

                if (isJumping)
                {
                    movementDirection.y = m_jumpSpeed;
                    isJumping = false;
                }

                movementDirection = transform.TransformDirection(movementDirection);
            }

            movementDirection += Physics.gravity * Time.deltaTime;

            m_characterController.Move(movementDirection * Time.deltaTime);
        }

        private void UpdateDistanceToGround()
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, Vector3.down);
            
            if (Physics.Raycast(downRay, out hit)) distanceToGround = hit.distance;

            /*
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000))
            {
                distanceToGround = Vector3.Distance(transform.position, hit.point);
            }
            */
        }
    }
}
