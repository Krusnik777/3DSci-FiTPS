using UnityEngine;

namespace SciFiTPS
{
    public class EntityContextAction : EntityAnimationAction
    {
        public bool IsCanStart;

        public override void StartAction()
        {
            if (!IsCanStart) return;

            base.StartAction();
        }
    }
}
