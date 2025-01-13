using UnityEngine;

namespace Tests.States
{
    public class StateReport : MonoBehaviour
    {
        private State[] states;
        
        public State[] States => states;

        void Awake() => states = GetComponents<State>();
    }
}
