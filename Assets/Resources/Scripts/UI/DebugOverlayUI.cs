using UnityEngine;
using UnityEngine.InputSystem;

using Tests;

[RequireComponent(typeof(SceneObjectMapping))]
public class DebugOverlayUI : MonoBehaviour
{
    private SceneObjectMapping scene;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        scene = GetComponent<SceneObjectMapping>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Game.F1.performed += OnF1Intent;
        inputActions.Game.F2.performed += OnF2Intent;
        inputActions.Game.F3.performed += OnF3Intent;
    }

    void OnDisable() => inputActions.Disable();

    private void OnF1Intent(InputAction.CallbackContext context)
    {
        if (scene.Attributes == null) return;
        scene.Attributes.CycleView();
    }

    private void OnF2Intent(InputAction.CallbackContext context)
    {
        if (scene.Actuators == null) return;
        scene.Actuators.FlipView();
    }

    private void OnF3Intent(InputAction.CallbackContext context)
    {
        if (scene.Analytics == null) return;
        scene.Analytics.FlipView();
    }
}
