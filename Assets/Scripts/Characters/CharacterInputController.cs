using System.Collections.Generic;
using UnityEngine;

namespace SciFiTPS
{
    public class CharacterInputController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private EntityActionCollector m_actionCollector;
        [SerializeField] private ThirdPersonCamera m_thirdPersonCamera;
        [SerializeField] private PlayerShooter m_playerShooter;
        [SerializeField] private Vector3 m_aimingOffset;

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

            if (Input.GetButtonDown("Use"))
            {
                if (!m_characterMovement.IsAiming && !m_characterMovement.IsCrouching)
                {
                    List<EntityContextAction> actionList = m_actionCollector.GetActionList<EntityContextAction>();

                    for (int i = 0; i < actionList.Count; i++)
                    {
                        actionList[i].StartAction();
                    }
                }
            }

            if (Input.GetButton("Fire1"))
            {
                if (m_characterMovement.IsAiming)
                    m_playerShooter.Shoot();
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
