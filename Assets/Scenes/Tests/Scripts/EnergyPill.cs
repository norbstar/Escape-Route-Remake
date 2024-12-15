using UnityEngine;

namespace Tests
{
    public class EnergyPill : Pill
    {
        [Header("Health")]
        [SerializeField] int energyPoints = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // protected override void OnTriggered()
        // {
        //     base.OnTriggered();
        //     Debug.Log($"OnTriggered");
        // }

        public int EnergyPoints => energyPoints;

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}