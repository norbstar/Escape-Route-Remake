using UnityEngine;

namespace Tests
{
    public class EditorTest : MonoBehaviour
    {
        [SerializeField] [ReadOnly] float floatValue;

        [SerializeField] [ReadOnly] EnumCondition.FooEnum enumValue;

        [SerializeField] [ReadOnly] EnumCondition enumCondition;

        public float FloatValue { get => floatValue; set => floatValue = value; }
        
        public EnumCondition.FooEnum EnumValue { get => enumValue; set => enumValue = value; }

        public EnumCondition EnumCondition { get => enumCondition; set => enumCondition = value; }
    }
}