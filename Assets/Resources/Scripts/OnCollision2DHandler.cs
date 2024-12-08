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
    private new Collider2D collider;

    public void Awake() => collider = GetComponent<Collider2D>();

    public void Register(Events events) => this.events = events;

    public void OnCollisionEnter2D(Collision2D collision) => events?.Gained?.Invoke(this, collision);

    public void OnCollisionStay2D(Collision2D collision) => events?.Sustained?.Invoke(this, collision);

    public void OnCollisionExit2D(Collision2D collision) => events?.Lost?.Invoke(this, collision);

    public bool Enabled { get => collider.enabled; set => collider.enabled = value; }
}