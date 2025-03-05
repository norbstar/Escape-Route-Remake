using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnTrigger2DHandler : MonoBehaviour
{
    public delegate void OnGained(OnTrigger2DHandler instance, Collider2D collider);
    public delegate void OnSustained(OnTrigger2DHandler instance, Collider2D collider);
    public delegate void OnLost(OnTrigger2DHandler instance, Collider2D collider);

    public class Events
    {
        public OnGained Gained { get; set; }
        public OnSustained Sustained { get; set; }
        public OnLost Lost { get; set; }
    }

    private Events events;
    private LayerMask layerMask;
    private new Collider2D collider;

    public void Awake() => collider = GetComponent<Collider2D>();

    public void Subscribe(Events events, LayerMask layerMask)
    {
        this.events = events;
        this.layerMask = layerMask;
    }

    public void Unsubscribe()
    {
        events = null;
        layerMask = new LayerMask();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            events?.Gained?.Invoke(this, collider);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            events?.Sustained?.Invoke(this, collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            events?.Lost?.Invoke(this, collider);
        }
    }

    public bool Enabled { get => collider.enabled; set => collider.enabled = value; }
}