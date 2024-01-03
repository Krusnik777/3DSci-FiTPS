using UnityEngine;

namespace SciFiTPS
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private ThirdPersonCamera m_thirdPersonCamera;
        [SerializeField] private Vector3 m_aimingOffset;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            m_characterMovement.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            m_thirdPersonCamera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            if (m_characterMovement.TargetDirectionControl != Vector3.zero || m_characterMovement.IsAiming)
            {
                m_thirdPersonCamera.IsRotateTarget = true;
            }
            else
            {
                m_thirdPersonCamera.IsRotateTarget = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                m_characterMovement.Aim();
                m_thirdPersonCamera.SetTargetOffset(m_aimingOffset);
            }
            if (Input.GetMouseButtonUp(1))
            {
                m_characterMovement.UnAim();
                m_thirdPersonCamera.SetDefaultOffset();
            }

            if (Input.GetButtonDown("Jump")) m_characterMovement.Jump();

            if (Input.GetKeyDown(KeyCode.LeftControl)) m_characterMovement.Crouch();
            if (Input.GetKeyUp(KeyCode.LeftControl)) m_characterMovement.UnCrouch();

            if (Input.GetKeyDown(KeyCode.LeftShift)) m_characterMovement.Sprint();
            if (Input.GetKeyUp(KeyCode.LeftShift)) m_characterMovement.UnSprint();
        }
    }
}
