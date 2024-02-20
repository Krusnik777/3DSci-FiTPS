using UnityEngine;

namespace SciFiTPS
{
    public class EntityAnimationAction : EntityAction
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private string m_actionAnimationName;
        [SerializeField] private float m_timeDuration;
        [SerializeField] private bool m_endActionByAnimation;

        public Animator Animator => m_animator;

        private Timer m_timer;

        private bool isPlayingAnimation;

        private void OnDestroy()
        {
            if (m_timer != null)
                m_timer.EventOnTick -= OnTimerTick;
        }

        public override void StartAction()
        {
            base.StartAction();

            m_animator.CrossFade(m_actionAnimationName, m_timeDuration);

            m_timer = Timer.CreateTimer(m_timeDuration, true);

            m_timer.EventOnTick += OnTimerTick;

        }

        public override void EndAction()
        {
            m_timer.EventOnTick -= OnTimerTick;

            base.EndAction();
        }

        private void OnTimerTick()
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionAnimationName))
            {
                isPlayingAnimation = true;
            }

            if (isPlayingAnimation)
            {
                if ((!m_endActionByAnimation && !m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionAnimationName)) || (m_endActionByAnimation && m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionAnimationName) && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99))
                {
                    isPlayingAnimation = false;

                    EndAction();
                }
            }
        }
    }
}
