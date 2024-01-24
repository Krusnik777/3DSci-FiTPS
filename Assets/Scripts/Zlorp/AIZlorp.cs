using System.Collections;
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
            CircleRandom
        }

        [SerializeField] private AIBehaviour m_aIBehaviour;
        [SerializeField] private Zlorp m_zlorp;
        [SerializeField] private CharacterMovement m_characterMovement;
        [SerializeField] private NavMeshAgent m_agent;
        [SerializeField] private PatrolPath m_patrolPath;
        [SerializeField] private int m_patrolPathNodeIndex = 0;

        private NavMeshPath m_navMeshPath;
        private PatrolPathNode currentPathNode;

        private void Start()
        {
            m_characterMovement.UpdatePosition = false;
            m_navMeshPath = new NavMeshPath();

            StartBehaviour(m_aIBehaviour);
        }

        private void Update()
        {
            SyncAgentAndCharacterMovement();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_aIBehaviour == AIBehaviour.Idle)
            {
                return;
            }

            if (m_aIBehaviour == AIBehaviour.PatrolRandom)
            {
                if (AgentReachedDestination() == true)
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }

            if (m_aIBehaviour == AIBehaviour.CircleRandom)
            {
                if (AgentReachedDestination() == true)
                {
                    StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
                }
            }
        }

        private void StartBehaviour(AIBehaviour state)
        {
            if (state == AIBehaviour.Idle)
            {
                m_agent.isStopped = true;
            }

            if (state == AIBehaviour.PatrolRandom)
            {
                m_agent.isStopped = false;
                SetDestinationByPathNode(m_patrolPath.GetRandomPathNode());
            }

            if (state == AIBehaviour.CircleRandom)
            {
                m_agent.isStopped = false;
                SetDestinationByPathNode(m_patrolPath.GetNextNode(ref m_patrolPathNodeIndex));
            }

            m_aIBehaviour = state;
        }

        private void SetDestinationByPathNode(PatrolPathNode node)
        {
            currentPathNode = node;

            m_agent.CalculatePath(node.transform.position, m_navMeshPath);
            m_agent.SetPath(m_navMeshPath);
        }

        private bool AgentReachedDestination()
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

        private void SyncAgentAndCharacterMovement()
        {
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

        #endregion

    }
}
