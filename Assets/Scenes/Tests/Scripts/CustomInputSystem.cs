using UnityEngine;

public class CustomInputSystem : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private bool isEnabled;

    void Awake() => inputActions = new InputSystem_Actions();

    private void Enable()
    {
        inputActions.Enable();
        isEnabled = true;
    }

    private void Disable()
    {
        inputActions.Disable();
        isEnabled = false;
    }

    void OnEnable() => Enable();

    void OnDisable() => Disable();

    public InputSystem_Actions InputActions => inputActions;

    public void Enable(bool enable)
    {
        if (enable)
        {
            Enable();
            return;
        }

        Disable();
    }

    public bool IsEnabled() => isEnabled;
}
