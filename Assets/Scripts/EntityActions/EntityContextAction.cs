using UnityEngine;

namespace SciFiTPS
{
    public class EntityContextAction : EntityAnimationAction
    {
        public bool IsCanStart;
        public bool IsCanEnd;

        public override void StartAction()
        {
            if (!IsCanStart) return;

            IsCanStart = false;

            base.StartAction();
        }

        public override void EndAction()
        {
            if (!IsCanEnd) return;

            IsCanEnd = false;

            base.EndAction();
        }
    }
}
