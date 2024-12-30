using System;

using UnityEngine;

namespace Tests
{
    public class FlagTest : MonoBehaviour
    {
        [SerializeField] ObjectPropertyEnum property;
        // [SerializeField] TestEnum test;

        // [Flags]
        // private enum TestEnum
        // {
        //     Foo = 1,
        //     Bar = 2,
        //     All = Foo | Bar
        // }

        // private PlayerPropertyEnum property;
        // private TestEnum localTest;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // property = new PlayerPropertyEnum();
            // property = property.Set(PlayerPropertyEnum.Gripable | PlayerPropertyEnum.Traversable);
            // property = PlayerPropertyEnum.Traversable;

            var flags = ObjectPropertyEnum.Gripable/* | PlayerPropertyEnum.Traversable*/;

            // Debug.Log($"Local Test Value: {localTest}");
            // Debug.Log($"Test Value: {test}");
            Debug.Log($"LValue: {Convert.ToInt64(property)}");
            Debug.Log($"FValue: {Convert.ToInt64(flags)}");

            // Debug.Log($"{lValue | Convert.ToInt64(PlayerPropertyEnum.Gripable)}");

            var hasFlag = property.HasFlag(flags);
            Debug.Log($"HasFlags: {flags} {hasFlag}");
        }
    }
}
