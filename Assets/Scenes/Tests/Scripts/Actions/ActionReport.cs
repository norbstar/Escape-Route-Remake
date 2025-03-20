using UnityEngine;

namespace Tests.Actions
{
    public class ActionReport : MonoBehaviour
    {
        [SerializeField] Action[] actions;
        
        public Action[] Actions => actions;

        void Awake() => actions = GetComponents<Action>();
    }
}
