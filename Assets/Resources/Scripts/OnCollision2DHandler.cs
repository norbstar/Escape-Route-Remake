using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnCollision2DHandler : MonoBehaviour
{
    public delegate void OnGained(OnCollision2DHandler instance, Collision2D collider);
    public delegate void OnSustained(OnCollision2DHandler instance, Collision2D collider);
    public delegate void OnLost(OnCollision2DHandler instance, Collision2D collider);

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

    public void Register(Events events, LayerMask layerMask)
    {
        this.events = events;
        this.layerMask = layerMask;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            events?.Gained?.Invoke(this, collision);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            events?.Sustained?.Invoke(this, collision);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if ((layerMask.value & (1 << collider.gameObject.layer)) > 0)
        {
            events?.Lost?.Invoke(this, collision);
        }
    }

    public bool Enabled { get => collider.enabled; set => collider.enabled = value; }
}