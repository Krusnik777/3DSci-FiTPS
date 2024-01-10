using UnityEngine;

namespace SciFiTPS
{
    public class DroneMovement : MonoBehaviour
    {
        [SerializeField] private CubeArea m_area;
        [SerializeField] private float m_restTime;
        [SerializeField] private bool m_moveAtStart;
        [Header("Movement")]
        [SerializeField] private float m_moveSpeed = 1.25f;
        [SerializeField] private float m_turnSpeed = 5f;

        private float timer;

        private bool isMoving = false;
        private bool isTurning = false;

        private Vector3 m_targetPosition;
        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        private void SetMovementTarget() => m_targetPosition = m_area.GetRandomInsideZone();
        private void CalculateHeadingDirection(Vector3 targetPosition) => headingDirection = (targetPosition - transform.position).normalized;
        private void SetHorizontalVelocity() => velocity = headingDirection * m_moveSpeed;

        public void SetMovementArea(CubeArea area)
        {
            m_area = area;
        }

        private void Start()
        {
            currentDirection = transform.forward;
        }

        private void Update()
        {
            if (!isMoving)
            {
                if (m_moveAtStart)
                {
                    SetMovementTarget();
                    isMoving = true;
                    m_moveAtStart = false;
                    return;
                }

                timer += Time.deltaTime;

                if (timer > m_restTime)
                {
                    SetMovementTarget();
                    isMoving = true;

                    timer = 0;
                }
            }
            else
            {
                if (!isTurning) Move();
                else Turn();
            }
        }

        private void Move()
        {
            if (Vector3.Distance(transform.position, m_targetPosition) >= 0.05f)
            {
                CalculateHeadingDirection(m_targetPosition);
                SetHorizontalVelocity();

                if (currentDirection != headingDirection)
                {
                    isTurning = true;
                    return;
                }

                //transform.forward = headingDirection;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = m_targetPosition;
                isMoving = false;
            }
        }

        private void Turn()
        {
            targetRotation = Quaternion.LookRotation(headingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);

            var angle = Quaternion.Angle(transform.rotation, targetRotation);

            if (angle <= 5)
            {
                transform.forward = headingDirection;
                currentDirection = transform.forward;
                isTurning = false;
            }
        }

    }
}
