using UnityEngine;

namespace SciFiTPS
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private Transform m_aim;
        [SerializeField] private float m_sensitive;
        [SerializeField] private float m_rotateTargetLerpRate;
        [Header("RotationLimit")]
        [SerializeField] private float m_minLimitY;
        [SerializeField] private float m_maxLimitY;
        [Header("Offset")]
        [SerializeField] private Vector3 m_offset;
        [SerializeField] private float m_offsetChangeRate = 5.0f;
        [Header("Distance")]
        [SerializeField] private float m_distance;
        [SerializeField] private float m_minDistance;
        [SerializeField] private float m_distanceLerpRate;
        [SerializeField] private float m_distanceHitOffset;

        [HideInInspector] public bool IsRotateTarget;
        [HideInInspector] public Vector2 RotationControl;

        public Transform Aim => m_aim;

        private float defaultMinRotationLimit;
        private float defaultMaxRotationLimit;

        private float deltaRotationX;
        private float deltaRotationY;

        private float currentDistance;

        private Vector3 targetOffset;
        private Vector3 defaultOffset;

        public void SetTargetOffset(Vector3 offset) => targetOffset = offset;
        public void SetDefaultOffset() => targetOffset = defaultOffset;

        public void SetDefaultRotationLimit()
        {
            m_minLimitY = defaultMinRotationLimit;
            m_maxLimitY = defaultMaxRotationLimit;
        }

        public void SetRotationLimit(float minRotationLimit, float maxRotationLimit)
        {
            m_minLimitY = minRotationLimit;
            m_maxLimitY = maxRotationLimit;
        }

        public void SetTarget(Transform target) => m_target = target;

        private void Start()
        {
            defaultOffset = m_offset;
            targetOffset = m_offset;

            defaultMinRotationLimit = m_minLimitY;
            defaultMaxRotationLimit = m_maxLimitY;

            transform.SetParent(null);
        }

        private void Update()
        {
            // Calculate rotation and translation
            deltaRotationX += RotationControl.x * m_sensitive;
            deltaRotationY += RotationControl.y * m_sensitive;

            deltaRotationY = ClampAngle(deltaRotationY, m_minLimitY, m_maxLimitY);

            m_offset = Vector3.MoveTowards(m_offset, targetOffset, m_offsetChangeRate * Time.deltaTime);
            //m_offset = Vector3.Slerp(m_offset, targetOffset, m_offsetChangeRate * Time.deltaTime);

            //Quaternion finalRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(deltaRotationY, deltaRotationX, 0), 0.25f);
            Quaternion finalRotation = Quaternion.Euler(deltaRotationY, deltaRotationX, 0);
            Vector3 finalPosition = m_target.position - (finalRotation * Vector3.forward * m_distance);
            finalPosition = AddLocalOffset(finalPosition);

            // Calculate current distance
            float targetDistance = m_distance;

            RaycastHit hit;

            Debug.DrawLine(m_target.position + new Vector3(0, m_offset.y, 0), finalPosition, Color.red);

            if (Physics.Linecast(m_target.position + new Vector3(0, m_offset.y, 0), finalPosition, out hit))
            {
                //float distanceToHit = Vector3.Distance(m_target.position + new Vector3(0, m_offset.y, 0), hit.point);
                float distanceToHit = hit.distance;

                if (hit.transform != m_target)
                {
                    if (distanceToHit < m_distance)
                        targetDistance = distanceToHit - m_distanceHitOffset;
                }
            }

            currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, Time.deltaTime * m_distanceLerpRate);
            currentDistance = Mathf.Clamp(currentDistance, m_minDistance, m_distance);

            // Correct camera position
            finalPosition = m_target.position - (finalRotation * Vector3.forward * currentDistance);

            // Apply transform
            transform.rotation = finalRotation;
            transform.position = finalPosition;
            transform.position = AddLocalOffset(transform.position);

            // Rotate target
            if (IsRotateTarget)
            {
                Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
                m_target.rotation = Quaternion.RotateTowards(m_target.rotation, targetRotation, Time.deltaTime * m_rotateTargetLerpRate);
                //m_target.rotation = Quaternion.Slerp(m_target.rotation, targetRotation, Time.deltaTime * m_rotateTargetLerpRate);
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
