using UnityEngine;

namespace SciFiTPS
{
    public class ListenTarget : MonoBehaviour
    {
        public enum ListenType
        {
            Character
        }

        [SerializeField] private ListenType m_type;
        public ListenType Type => m_type;

        public bool IsHearableFromPoint(Vector3 point, float range)
        {
            if (Vector3.Distance(transform.position, point) <= range)
                return true;

            return false; 
        }

    }
}
