using UnityEngine;

namespace Tests.Events
{
    public class EventReport : MonoBehaviour
    {
        [SerializeField] Event[] events;
        
        public Event[] Events => events;

        void Awake() => events = GetComponents<Event>();
    }
}
