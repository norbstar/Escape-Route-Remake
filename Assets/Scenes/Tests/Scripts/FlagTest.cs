using System;

using UnityEngine;

namespace Tests
{
    public class FlagTest : MonoBehaviour
    {
        [SerializeField] PlayerPropertyEnum property;
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
            // property = property.Set(PlayerPropertyEnum.Sticky | PlayerPropertyEnum.Other);
            // property = PlayerPropertyEnum.Other;

            if (Convert.ToInt64(property) == -1)
            {

            }

            var flags = PlayerPropertyEnum.Sticky/* | PlayerPropertyEnum.Other*/;

            // Debug.Log($"Local Test Value: {localTest}");
            // Debug.Log($"Test Value: {test}");
            Debug.Log($"LValue: {Convert.ToInt64(property)}");
            Debug.Log($"FValue: {Convert.ToInt64(flags)}");

            // Debug.Log($"{lValue | Convert.ToInt64(PlayerPropertyEnum.Sticky)}");

            var hasFlag = property.HasFlag(flags);
            Debug.Log($"HasFlags: {flags} {hasFlag}");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
