using UnityEngine;

namespace SciFiTPS
{
    public class CharacterInputController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private ThirdPersonCamera m_thirdPersonCamera;
        [SerializeField] private CameraShooter m_playerShooter;
        [SerializeField] private SpreadShootRig m_spreadShootRig;
        [SerializeField] private Vector3 m_aimingOffset;

        public void AssignCamera(ThirdPersonCamera camera)
        {
            m_thirdPersonCamera = camera;
            m_thirdPersonCamera.SetDefaultOffset();
            m_thirdPersonCamera.SetDefaultRotationLimit();
            m_thirdPersonCamera.SetTarget(m_characterMovement.transform);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (m_characterMovement.IsInteractiong)
            {
                if (m_thirdPersonCamera.RotationControl != Vector2.zero) m_thirdPersonCamera.RotationControl = Vector2.zero;
                return;
            }

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

            if (Input.GetButton("Fire1"))
            {
                if (m_characterMovement.IsAiming)
                {
                    if (m_playerShooter.Weapon.CanFire)
                    {
                        m_playerShooter.Shoot();
                        m_spreadShootRig.Spread();
                    }
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                m_characterMovement.Aim();
                m_thirdPersonCamera.SetTargetOffset(m_aimingOffset);
            }
            if (Input.GetButtonUp("Fire2"))
            {
                m_characterMovement.UnAim();
                m_thirdPersonCamera.SetDefaultOffset();
            }

            if (Input.GetButtonDown("Jump")) m_characterMovement.Jump();

            if (Input.GetButtonDown("Crouch")) m_characterMovement.Crouch();
            if (Input.GetButtonUp("Crouch")) m_characterMovement.UnCrouch();

            if (Input.GetButtonDown("Sprint")) m_characterMovement.Sprint();
            if (Input.GetButtonUp("Sprint")) m_characterMovement.UnSprint();
        }
    }
}
