using UnityEngine;

namespace Tests
{
    public class SpecialAbilityPill : Pill
    {
        public enum AbilityEnum
        {
            PowerJump,
            Dash
        }

        [Header("Ability")]
        [SerializeField] AbilityEnum ability;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // protected override void OnTriggered()
        // {
        //     base.OnTriggered();
        //     Debug.Log($"OnTriggered");
        // }

        public AbilityEnum Ability => ability;

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}