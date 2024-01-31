using System.Collections;
using UnityEngine;

namespace SciFiTPS
{
    public class Climbing_Ladder : ActionInteract
    {
        [Header("Ladder")]
        [SerializeField] private string m_actionEndAnimationName;
        [SerializeField] private float m_climbSpeed = 2.0f;
        [SerializeField] private float m_finalClimbSpeed = 1.0f;
        [SerializeField] private float m_finalMoveFactor = 2.0f;

        public override void EndAction()
        {
            if (!IsCanEnd) return;

            StartCoroutine(MoveToEndPosition());
        }

        #region Coroutines

        private IEnumerator MoveToEndPosition()
        {
            var startPosition = m_owner.position;
            var targetPosition = startPosition + new Vector3(0, Properties.EndPoint.position.y, 0);

            CharacterMovement movement = m_owner.GetComponent<CharacterMovement>();
            movement.Climb();

            while (targetPosition.y - m_owner.position.y >= 0.05f)
            {
                m_owner.position += m_owner.transform.up * m_climbSpeed * Time.deltaTime; 

                yield return null;
            }

            Animator.SetTrigger(m_actionEndAnimationName);

            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionEndAnimationName));

            yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f);

            startPosition = m_owner.position;
            targetPosition = Properties.FinalPosition.position;

            while (targetPosition.y - m_owner.position.y >= 0.01f)
            {
                m_owner.position += m_owner.transform.up * m_finalClimbSpeed * Time.deltaTime;

                yield return null;
            }

            startPosition = m_owner.position = new Vector3(m_owner.position.x, targetPosition.y, m_owner.position.z);

            var elapsed = 0.0f;

            while (Vector3.Distance(m_owner.position, targetPosition) >= 0.05f)
            {
                m_owner.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed /m_climbSpeed * m_finalMoveFactor);

                elapsed += Time.deltaTime;

                yield return null;
            }

            yield return new WaitWhile(() => Animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionEndAnimationName));

            m_owner.position = Properties.FinalPosition.position;

            movement.UnClimb();

            base.EndAction();
        }

        #endregion
    }
}
