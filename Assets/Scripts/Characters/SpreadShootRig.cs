using UnityEngine;

namespace SciFiTPS
{
    public class SpreadShootRig : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animations.Rigging.Rig m_spreadRig;
        [SerializeField] private float m_changeWeightLerpRate;

        private float targetWeight;

        private bool isFired = false;
        public bool IsFired => isFired;

        public void Spread()
        {
            targetWeight = 1;
            isFired = true;
        }

        private void Update()
        {
            m_spreadRig.weight = Mathf.MoveTowards(m_spreadRig.weight, targetWeight, Time.deltaTime * m_changeWeightLerpRate);

            if (m_spreadRig.weight == 1)
            {
                targetWeight = 0;
                isFired = false;
            }
        }
    }
}
