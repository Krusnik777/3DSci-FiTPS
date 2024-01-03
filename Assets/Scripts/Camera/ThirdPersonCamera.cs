using UnityEngine;

namespace SciFiTPS
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private float m_distance;
        [SerializeField] private float m_sensitive;
        [Header("RotationLimit")]
        [SerializeField] private float m_minLimitY;
        [SerializeField] private float m_maxLimitY;
        [SerializeField] private Vector3 m_offset;

        [HideInInspector] public bool IsRotateTarget;
        [HideInInspector] public Vector2 RotationControl;

        private float deltaRotationX;
        private float deltaRotationY;

        private Vector3 targetOffset;
        private Vector3 defaultOffset;

        public void SetTargetOffset(Vector3 offset)
        {
            targetOffset = offset;
            m_offset = offset;
        }
        public void SetDefaultOffset()
        {
            targetOffset = defaultOffset;
            m_offset = defaultOffset;
        }

        private void Start()
        {
            defaultOffset = m_offset;
            targetOffset = m_offset;
        }

        private void Update()
        {
            deltaRotationX += RotationControl.x * m_sensitive;
            deltaRotationY += RotationControl.y * m_sensitive;

            deltaRotationY = ClampAngle(deltaRotationY, m_minLimitY, m_maxLimitY);

            Quaternion finalRotation = Quaternion.Euler(deltaRotationY, deltaRotationX, 0);
            Vector3 finalPosition = m_target.position - (finalRotation * Vector3.forward * m_distance);

            finalPosition = AddLocalOffset(finalPosition);

            transform.position = finalPosition;
            transform.rotation = finalRotation;

            if (IsRotateTarget)
            {
                m_target.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }

        private float ClampAngle(float angle, float min, float max)
        {
            /*
            angle %= 360;
            angle = angle > 180 ? angle - 360 : angle;*/

            if (angle < - 360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }

        private Vector3 AddLocalOffset(Vector3 position)
        {
            Vector3 result = position;
            result.y += m_offset.y;

            result += transform.right * m_offset.x;
            result += transform.forward * m_offset.z;

            return result;
        }

    }
}
