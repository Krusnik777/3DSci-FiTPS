using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SciFiTPS
{
    public class AIZlorp : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Idle,
            PatrolRandom,
            CirclePatrol,
            PursueTarget,
            SeekTarget
        }

        [SerializeField] private AIBehaviour m_aIBehaviour;
        [SerializeField] private Zlorp m_zlorp;
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private NavMeshAgent m_agent;
        [SerializeField] private PatrolPath m_patrolPath;
        [SerializeField] private ColliderViewer m_colliderViewer;
        [SerializeField] private float m_aimingDistance;
        [SerializeField] private int m_patrolPathNodeIndex = 0;
        [Header("Search")]
        [SerializeField] private float m_searchTime = 10f;
        [SerializeField] private float m_searchRange = 7f;
        [SerializeField] private float m_lookAroundTime = 2f;

        private NavMeshPath m_navMeshPath;
        private PatrolPathNode currentPathNode;

        private GameObject potentialTarget;
        private Transform pursueTarget;
        private Vector3 seekTarget;

        private Coroutine searchRoutine;

        public void SetPursueTarget(Transform target) => pursueTarget = target;

        private bool CheckAgentReachedDestination()
        {
            if (!m_agent.pathPending)
            {
                if (m_agent.remainingDistance <= m_agent.stoppingDistance)
                {
                    if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0.0f) return true;

                    return false;
                }

                return false;
            }

            return false;
        }

        private bool TryGetRandomPoint(Vector3 center, float range, out Vector3 result)
        {
            // 30 tries to find point

            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;

                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }

            result = Vector3.zero;
            return false;
        }

        private void Start()
        {
            potentialTarget = Destructible.FindNearestNonTeamMember(m_zlorp)?.gameObject;

            m_characterMovement.UpdatePosition = false;
            m_navMeshPath = new NavMeshPath();

            StartBehaviour(m_aIBehaviour);

            m_zlorp.EventOnDamaged += OnGetDamage;
        }

        private void OnDestroy()
        {
            m_zlorp.EventOnDamaged -= OnGetDamage;
        }

        private void Update()
        {
            SyncAgentAndCharacterMovement();

            UpdateAI();
        }

        private void OnGetDamage(Destructible other)
        {
            if (other.TeamId != m_zlorp.TeamId)
            {
                ActionAssignTargetAllTeammates(other.transform);
            }

        }

        private void UpdateAI()
        {
            ActionUpdateTarget();

            if (m_aIBehaviour == AIBehaviour.Idle)
            {
                return;
            }

            if (m_aIBehaviour == AIBehaviour.PursueTarget)
            {
                m_agent.CalculatePath(pursueTarget.position, m_navMeshPath);
                m_agent.SetPath(m_navMeshPath);

                if (Vector3.Distance(transform.position, pursueTarget.position) <= m_aimingDistance)
                {
                    m_characterMovement.Aim();

                    m_zlorp.Fire(pursueTarget.position + new Vector3(0, 1, 0));
                }
                else m_characterMovement.UnAim();

            }

            if (m_aIBehaviour == AIBehaviour.SeekTarget)
            {
                if (searchRoutine == null)
                {
                    m_agent.CalculatePath(seekTarget, m_navMeshPath);
                    m_agent.SetPath(m_navMeshPath);

                    if (CheckAgentReachedDestination())
                    {
                        searchRoutine = StartCoroutine(SearchAround());
                    }
                }
                else
                {
                    if (m_agent.remainingDistance <= m_agent.stoppingDistance && !m_agent.isStopped)
                    {
                        StartCoroutine(LookAroundAndSetNextPoint(m_lookAroundTime));
                    }
                }
            }

            if (m_aIBehaviour == AIBehaviour.PatrolRandom)
            {
                if (CheckAgentReachedDestination())
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }

            if (m_aIBehaviour == AIBehaviour.CirclePatrol)
            {
                if (CheckAgentReachedDestination())
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }
        }

        private void ActionUpdateTarget()
        {
            if (potentialTarget == null) return;

            if (m_colliderViewer.IsObjectVisible(potentialTarget))
            {
                if (searchRoutine != null)
                {
                    StopCoroutine(searchRoutine);
                    searchRoutine = null;
                }

                pursueTarget = potentialTarget.transform;
                ActionAssignTargetAllTeammates(pursueTarget);
            }
            else
            {
                if (pursueTarget != null)
                {
                    seekTarget = pursueTarget.position;
                    pursueTarget = null;
                    StartBehaviour(AIBehaviour.SeekTarget);
                }
            }
        }

        private void ActionAssignTargetAllTeammates(Transform other)
        {
            List<Destructible> teammates = Destructible.GetAllTeamMembers(m_zlorp.TeamId);

            foreach (var dest in teammates)
            {
                AIZlorp ai = dest.transform.root.GetComponent<AIZlorp>();

                if (ai != null && ai.enabled)
                {
                    ai.SetPursueTarget(other);
                    ai.StartBehaviour(AIBehaviour.PursueTarget);
                }
            }
        }

        private void StartBehaviour(AIBehaviour state)
        {
            if (m_zlorp.IsDead) return;

            if (state == AIBehaviour.Idle)
            {
                m_agent.isStopped = true;
                m_characterMovement.UnAim();
            }

            if (state == AIBehaviour.PatrolRandom)
            {
                m_agent.isStopped = false;
                m_characterMovement.UnAim();
                SetDestinationByPathNode(m_patrolPath.GetRandomPathNode());
            }

            if (state == AIBehaviour.CirclePatrol)
            {
                m_agent.isStopped = false;
                m_characterMovement.UnAim();
                SetDestinationByPathNode(m_patrolPath.GetNextNode(ref m_patrolPathNodeIndex));
            }

            if (state == AIBehaviour.PursueTarget)
            {
                m_agent.isStopped = false;
            }

            if (state == AIBehaviour.SeekTarget)
            {
                m_agent.isStopped = false;
                m_characterMovement.UnAim();
            }

            m_aIBehaviour = state;
        }

        private void SetDestinationByPathNode(PatrolPathNode node)
        {
            currentPathNode = node;

            m_agent.CalculatePath(node.transform.position, m_navMeshPath);
            m_agent.SetPath(m_navMeshPath);
        }

        private void SyncAgentAndCharacterMovement()
        {
            m_agent.speed = m_characterMovement.CurrentSpeed;

            float factor = m_agent.velocity.magnitude / m_agent.speed;
            m_characterMovement.TargetDirectionControl = transform.InverseTransformDirection(m_agent.velocity.normalized) * factor;
        }

        #region Coroutines

        private IEnumerator SetBehaviourOnTime(AIBehaviour state, float seconds)
        {
            AIBehaviour prevBehaviour = m_aIBehaviour;
            m_aIBehaviour = state;
            StartBehaviour(m_aIBehaviour);

            yield return new WaitForSeconds(seconds);

            StartBehaviour(prevBehaviour);
        }

        private IEnumerator SearchAround()
        {         
            yield return new WaitForSeconds(m_searchTime);

            StartBehaviour(AIBehaviour.PatrolRandom);

            searchRoutine = null;
        }

        private IEnumerator LookAroundAndSetNextPoint(float seconds)
        {
            m_agent.isStopped = true;

            yield return new WaitForSeconds(seconds);

            if (TryGetRandomPoint(m_agent.transform.position, m_searchRange, out seekTarget))
            {
                m_agent.CalculatePath(seekTarget, m_navMeshPath);
                m_agent.SetPath(m_navMeshPath);
            }

            m_agent.isStopped = false;
        }

        #endregion

    }
}
