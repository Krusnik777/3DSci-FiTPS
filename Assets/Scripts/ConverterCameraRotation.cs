using UnityEngine;

namespace SciFiTPS
{
    public class ConverterCameraRotation : MonoBehaviour
    {
        [SerializeField] private Transform m_camera;
        [SerializeField] private Transform m_cameraLook;
        [SerializeField] private Vector3 m_lookOffset;
        [SerializeField] private float m_topAngleLimit;
        [SerializeField] private float m_bottomAngleLimit;

        private void Update()
        {
            Vector3 angles = new Vector3(0, 0, 0);

            angles.z = m_camera.eulerAngles.x;

            if (angles.z >= m_topAngleLimit || angles.z <= m_bottomAngleLimit)
            {
                transform.LookAt(m_cameraLook.position + m_lookOffset);

                angles.x = transform.eulerAngles.x;
                angles.y = transform.eulerAngles.y;

                transform.eulerAngles = angles;
            }
        }
    }
}
