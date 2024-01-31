using UnityEngine;

namespace SciFiTPS
{
    [System.Serializable]
    public class ActionUseVehicleProperties : ActionInteractProperties
    {
        public Vehicle Vehicle;
        public VehicleInputControl VehicleInputControl;
        public GameObject Hint;
        public ExitPointFinder ExitPointFinder;
    }

    public class ActionUseVehicle : ActionInteract
    {
        [Header("ActionComponents")]
        [SerializeField] private CharacterController m_characterController;
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private CharacterInputController m_characterInputController;
        [SerializeField] private GameObject m_visualModel;
        [SerializeField] private ThirdPersonCamera m_camera;

        private bool inVehicle;

        private void Start()
        {
            EventOnStart.AddListener(OnActionStart);
            EventOnEnd.AddListener(OnActionEnd);
        }

        private void OnDestroy()
        {
            EventOnStart.RemoveListener(OnActionStart);
            EventOnEnd.RemoveListener(OnActionEnd);
        }

        private void Update()
        {
            if (inVehicle)
            {
                IsCanEnd = (Properties as ActionUseVehicleProperties).Vehicle.LinearVelocity < 2;
            }
        }

        private void OnActionStart()
        {
            inVehicle = true;

            ActionUseVehicleProperties props = Properties as ActionUseVehicleProperties;

            // Camera
            props.VehicleInputControl.AssignCamera(m_camera);

            // Input
            props.VehicleInputControl.enabled = true;
            m_characterInputController.enabled = false;

            // CharacterMovement
            m_characterController.enabled = false;
            m_characterMovement.enabled = false;

            // Hide Visual Model
            m_visualModel.transform.localPosition = m_visualModel.transform.localPosition + new Vector3(0, 100000, 0);

            // Hide Hint
            props.Hint.SetActive(false);
        }

        private void OnActionEnd()
        {
            inVehicle = false;

            ActionUseVehicleProperties props = Properties as ActionUseVehicleProperties;

            // Camera
            m_characterInputController.AssignCamera(m_camera);

            // Input
            props.VehicleInputControl.enabled = false;
            m_characterInputController.enabled = true;

            // CharacterMovement
            var exit = props.ExitPointFinder.FindExitPoint();

            if (exit != null)
                m_owner.position = exit.position;
            else 
                m_owner.position = props.InteractTransform.position;

            m_characterController.enabled = true;
            m_characterMovement.enabled = true;

            // Show Visual Model
            m_visualModel.transform.localPosition = new Vector3(0, 0, 0);

            // Show Hint
            props.Hint.SetActive(true);

        }
    }
}
