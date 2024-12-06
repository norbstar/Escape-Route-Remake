using UI;
using UnityEngine;

namespace Tests
{
    public class SceneObjectMapping : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] HealthBarUI healthBarUI;
        [SerializeField] EnergyBarUI energyBarUI;
        [SerializeField] Transform attributesUI;
        [SerializeField] Transform actuatorsUI;
        [SerializeField] Transform analyticsUI;

        public HealthBarUI HealthBar => healthBarUI;

        public EnergyBarUI EnergyBar => energyBarUI;

        public Transform Attributes => attributesUI;

        public Transform Actuators => actuatorsUI;

        public Transform Analytics => analyticsUI;
    }
}