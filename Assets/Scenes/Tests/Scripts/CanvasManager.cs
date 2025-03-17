using UnityEngine;

using UI;

namespace Tests
{
    public class CanvasManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] HealthBarUI healthBarUI;
        [SerializeField] EnergyBarUI energyBarUI;
        [SerializeField] AttributesUI attributesUI;
        [SerializeField] ActuatorsUI actuatorsUI;
        [SerializeField] AnalyticsUI analyticsUI;

        public HealthBarUI HealthBar => healthBarUI;

        public EnergyBarUI EnergyBar => energyBarUI;

        public AttributesUI Attributes => attributesUI;

        public ActuatorsUI Actuators => actuatorsUI;

        public AnalyticsUI Analytics => analyticsUI;
    }
}