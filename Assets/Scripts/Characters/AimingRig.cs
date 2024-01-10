using UnityEngine;

namespace SciFiTPS
{
    public class AimingRig : MonoBehaviour
    {
        [SerializeField] private CharacterMovement m_targetCharacter;
        [SerializeField] private UnityEngine.Animations.Rigging.Rig[] m_rigs;
        [SerializeField] private float m_changeWeightLerpRate;

        private float targetWeight;

        private void Update()
        {
            for (int i = 0; i < m_rigs.Length; i++)
            {
                m_rigs[i].weight = Mathf.MoveTowards(m_rigs[i].weight, targetWeight, Time.deltaTime * m_changeWeightLerpRate);
            }

            if (m_targetCharacter.IsAiming)
            {
                targetWeight = 1;
            }
            else targetWeight = 0;
        }
    }
}
