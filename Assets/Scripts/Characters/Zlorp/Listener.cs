using UnityEngine;

namespace SciFiTPS
{
    [System.Serializable]
    public class ListenZones
    {
        public float CloseRange;
        public float MidRange;
        public float FarRange;
    }

    public class Listener : MonoBehaviour
    {
        [SerializeField] private ListenZones m_listenZones;

        public bool IsHeardTarget(ListenTarget target)
        {
            if (target == null) return false;

            if (HearTargetInCloseRange(target)) return true;
            if (HearTargetInMidRange(target)) return true;
            if (HearTargetInFarRange(target)) return true;

            return false;
        }

        private bool HearTargetInCloseRange(ListenTarget target)
        {
            if (target.IsHearableFromPoint(transform.position, m_listenZones.CloseRange))
            {
                var targetMovement = target.GetComponent<CharacterMovement>();

                if (targetMovement == null) Debug.LogError("ListenTarget: CharacterMovement is not found");

                var spreadShootRig = target.GetComponentInChildren<SpreadShootRig>();

                if (spreadShootRig == null) Debug.LogError("ListenTarget: PlayerShooter is not found");

                if (targetMovement != null)
                    if (!targetMovement.IsNotMoving) return true;

                if (spreadShootRig != null)
                    if (spreadShootRig.IsFired) return true;
            }

            return false;
        }

        private bool HearTargetInMidRange(ListenTarget target)
        {
            if (target.IsHearableFromPoint(transform.position, m_listenZones.MidRange))
            {
                if (target.Type == ListenTarget.ListenType.Character)
                {
                    var targetMovement = target.GetComponent<CharacterMovement>();

                    if (targetMovement == null) Debug.LogError("ListenTarget: CharacterMovement is not found");

                    var spreadShootRig = target.GetComponentInChildren<SpreadShootRig>();

                    if (spreadShootRig == null) Debug.LogError("ListenTarget: PlayerShooter is not found");

                    if (targetMovement != null)
                        if (!targetMovement.IsCrouching && !targetMovement.IsNotMoving) return true;

                    if (spreadShootRig != null)
                        if (spreadShootRig.IsFired) return true;
                }
            }

            return false;
        }

        private bool HearTargetInFarRange(ListenTarget target)
        {
            if (target.IsHearableFromPoint(transform.position, m_listenZones.FarRange))
            {
                if (target.Type == ListenTarget.ListenType.Character)
                {
                    var spreadShootRig = target.GetComponentInChildren<SpreadShootRig>();

                    if (spreadShootRig == null) Debug.LogError("ListenTarget: PlayerShooter is not found");

                    if (spreadShootRig != null)
                        if (spreadShootRig.IsFired) return true;
                }
            }

            return false;
        }


        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_listenZones.CloseRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_listenZones.MidRange);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, m_listenZones.FarRange);
        }
        #endif
    }
}
