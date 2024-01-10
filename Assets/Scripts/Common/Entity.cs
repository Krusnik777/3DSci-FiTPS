using UnityEngine;

namespace SciFiTPS
{
    /// <summary>
    /// Base class for all interactive objects on scene
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// Object name for user
        /// </summary>
        [SerializeField] private string m_nickname;
        public string Nickname => m_nickname;
    }
}
