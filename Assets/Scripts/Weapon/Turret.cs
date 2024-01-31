using UnityEngine;

namespace SciFiTPS
{
    public class Turret : Weapon
    {
        [SerializeField] private Transform m_rotatingBaseTransform;
        [SerializeField] private Transform m_gunTransform;
        [SerializeField] private Transform m_aim;
        [SerializeField] private float m_rotationLerpFactor;

        protected Quaternion baseTargetRotation;
        protected Quaternion baseRotation;
        protected Quaternion gunTargetRotation;
        protected Vector3 gunRotation;

        public void SetAim(Transform aim) => m_aim = aim;

        protected override void Update()
        {
            base.Update();

            LookAtAim();
        }

        private void LookAtAim()
        {
            baseTargetRotation = Quaternion.LookRotation(new Vector3(m_aim.position.x, m_gunTransform.position.y, m_aim.position.z) - m_gunTransform.position);
            baseRotation = Quaternion.RotateTowards(m_rotatingBaseTransform.localRotation, baseTargetRotation, m_rotationLerpFactor * Time.deltaTime);
            m_rotatingBaseTransform.localRotation = baseRotation;

            gunTargetRotation = Quaternion.LookRotation(m_aim.position - m_rotatingBaseTransform.position);
            gunRotation = Quaternion.RotateTowards(m_gunTransform.rotation, gunTargetRotation, m_rotationLerpFactor * Time.deltaTime).eulerAngles;
            m_gunTransform.rotation = baseRotation * Quaternion.Euler(gunRotation.x, 0, 0);
        }

        
    }
}
