using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(Drone))]
    public class AIDrone : MonoBehaviour
    {
        [SerializeField] private CubeArea m_movementArea;
        [SerializeField] private float m_angryDistance;

        private Drone m_drone;

        private Vector3 m_targetPosition;
        private Transform m_shootTarget;

        private Transform m_player;

        private void SetMovementTarget() => m_targetPosition = m_movementArea.GetRandomInsideZone();

        public void SetMovementArea(CubeArea area)
        {
            m_movementArea = area;
        }

        private void Start()
        {
            m_drone = GetComponent<Drone>();
            m_drone.EventOnDeath.AddListener(OnDroneDeath);

            m_player = GameObject.FindGameObjectWithTag("Player").transform;

            //currentDirection = transform.forward; // For OLD
        }

        private void OnDestroy()
        {
            m_drone.EventOnDeath.RemoveListener(OnDroneDeath);
        }

        private void Update()
        {
            UpdateAI();
        }

        private void UpdateAI()
        {
            // Update movement position

            if (transform.position == m_targetPosition) SetMovementTarget();

            if (Physics.Linecast(transform.position, m_targetPosition)) SetMovementTarget();

            // Find Fire Position
            if (Vector3.Distance(transform.position, m_player.position) <= m_angryDistance)
            {
                m_shootTarget = m_player;
            }

            // Move
            m_drone.MoveTo(m_targetPosition);

            if (m_shootTarget != null)
            {
                m_drone.LookAt(m_shootTarget.position);
            }
            else m_drone.LookAt(m_targetPosition);

            // Fire

            if (m_shootTarget != null)
            {
                m_drone.Fire(m_shootTarget.position);
            }
        }

        private void OnDroneDeath()
        {
            enabled = false;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_angryDistance);
        }
        #endif

        #region OLD
        /*
        private float timer;

        private bool isMoving = false;
        private bool isTurning = false;

        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        private void CalculateHeadingDirection(Vector3 targetPosition) => headingDirection = (targetPosition - transform.position).normalized;
        private void SetHorizontalVelocity() => velocity = headingDirection * m_moveSpeed;

        private void OldUpdateAI()
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
        }*/

        #endregion
    }

}
