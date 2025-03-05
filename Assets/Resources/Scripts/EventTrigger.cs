using UnityEngine;

[RequireComponent(typeof(OnTrigger2DHandler))]
public class EventTrigger : MonoBehaviour
{
    private OnTrigger2DHandler handler;

    void Awake() => handler = GetComponent<OnTrigger2DHandler>();

    void OnEnable()
    {
        handler.Subscribe(new OnTrigger2DHandler.Events()
        {
            Gained = (instance, collider) => Debug.Log("Gained"),
            Sustained = (instance, collider) => Debug.Log("Sustained"),
            Lost = (instance, collider) => Debug.Log("Lost")
        }, new LayerMask() { value = 1 << LayerMask.NameToLayer("Player") });
    }

    void OnDisable() => handler.Unsubscribe();
}
