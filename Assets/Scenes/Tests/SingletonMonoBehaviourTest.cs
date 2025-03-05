using UnityEngine;

namespace Tests
{
    public class SingletonMonoBehaviourTest : SingletonMonoBehaviour<SingletonMonoBehaviourTest>
    {
        // Start is called before the first frame update
        public override void Awake()
        {
            Debug.Log($"Name: {name}");
            base.Awake();
        }
    }
}
