using UnityEngine;

namespace SciFiTPS
{
    public class EntityAnimationAction : EntityAction
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private string m_actionAnimationName;
        [SerializeField] private float m_timeDuration;

        private Timer m_timer;

        private bool isPlayingAnimation;

        public override void StartAction()
        {
            base.StartAction();

            m_animator.CrossFade(m_actionAnimationName, m_timeDuration);

            m_timer = Timer.CreateTimer(m_timeDuration, true);

            m_timer.EventOnTick += OnTimerTick;

        }

        public override void EndAction()
        {
            base.EndAction();

            m_timer.EventOnTick -= OnTimerTick;
        }

        private void OnTimerTick()
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionAnimationName))
            {
                isPlayingAnimation = true;
            }

            if (isPlayingAnimation)
            {
                if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_actionAnimationName))
                {
                    isPlayingAnimation = false;

                    EndAction();
                }
            }
        }
    }
}
