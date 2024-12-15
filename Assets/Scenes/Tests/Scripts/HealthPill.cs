using UnityEngine;

namespace Tests
{
    public class HealthPill : Pill
    {
        [Header("Health")]
        [SerializeField] int healthPoints = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // protected override void OnTriggered()
        // {
        //     base.OnTriggered();
        //     Debug.Log($"OnTriggered");
        // }

        public int HealthPoints => healthPoints;

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}