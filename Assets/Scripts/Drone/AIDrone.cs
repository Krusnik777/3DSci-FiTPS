using System.Collections.Generic;
using UnityEngine;

namespace SciFiTPS
{
    [RequireComponent(typeof(Drone))]
    public class AIDrone : MonoBehaviour
    {
        [SerializeField] private CubeArea m_movementArea;
        [SerializeField] private ColliderViewer m_colliderViewer;

        private Drone m_drone;

        private Vector3 m_targetPosition;
        private Transform m_shootTarget;

        private void SetMovementTarget() => m_targetPosition = m_movementArea.GetRandomInsideZone();
        private Transform FindShootTarget()
        {
            List<Destructible> targets = Destructible.GetAllNonTeamMembers(m_drone.TeamId);

            for (int i = 0; i < targets.Count; i++)
            {
                if (m_colliderViewer.IsObjectVisible(targets[i].gameObject))
                    return targets[i].transform;
            }

            return null;
        }

        public void SetMovementArea(CubeArea area) => m_movementArea = area;
        public void SetShootTarget(Transform target) => m_shootTarget = target;

        private void Start()
        {
            m_drone = GetComponent<Drone>();
            m_drone.EventOnDeath.AddListener(OnDroneDeath);
            m_drone.EventOnDamaged += OnGetDamage;

            //currentDirection = transform.forward; // For OLD
        }

        private void OnDestroy()
        {
            m_drone.EventOnDeath.RemoveListener(OnDroneDeath);
            m_drone.EventOnDamaged -= OnGetDamage;
        }

        private void Update()
        {
            UpdateAI();
        }

        private void UpdateAI()
        {
            ActionFindShootingTarget();

            ActionMove();

            ActionFire();
        }

        private void OnDroneDeath()
        {
            enabled = false;
        }

        private void OnGetDamage(Destructible other)
        {
            ActionAssignTargetAllTeammates(other.transform);
        }

        private void ActionFindShootingTarget()
        {
            Transform potentialTarget = FindShootTarget();

            if (potentialTarget != null) ActionAssignTargetAllTeammates(potentialTarget);
        }

        private void ActionMove()
        {
            if (transform.position == m_targetPosition) SetMovementTarget();

            if (Physics.Linecast(transform.position, m_targetPosition)) SetMovementTarget();

            m_drone.MoveTo(m_targetPosition);

            if (m_shootTarget != null)
            {
                m_drone.LookAt(m_shootTarget.position);
            }
            else m_drone.LookAt(m_targetPosition);
        }

        private void ActionFire()
        {
            if (m_shootTarget != null)
            {
                m_drone.Fire(m_shootTarget.position);
            }
        }

        private void ActionAssignTargetAllTeammates(Transform other)
        {
            List<Destructible> teammates = Destructible.GetAllTeamMembers(m_drone.TeamId);

            foreach (var dest in teammates)
            {
                AIDrone ai = dest.transform.root.GetComponent<AIDrone>();

                if (ai != null && ai.enabled)
                {
                    ai.SetShootTarget(other);
                }
            }
        }

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
