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
        inputActions.Game.F4.performed += OnF4Intent;
        // inputActions.Game.F5.performed += OnF5Intent;
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

    private void ShowAll()
    {
        if (scene.Attributes == null) return;
        scene.Attributes.SetView(AttributesUI.ViewEnum.All);

        if (scene.Actuators == null) return;
        scene.Actuators.Active = true;

        if (scene.Analytics == null) return;
        scene.Analytics.Active = true;
    }

    private bool AnyActive()
    {
        bool active = false;

        if (scene.Attributes != null)
        {
            active = scene.Attributes.Active;
        }

        if (!active)
        {
            if (scene.Actuators != null)
            {
                active = scene.Actuators.Active;
            }
        }

        if (!active)
        {
            if (scene.Analytics != null)
            {
                active = scene.Analytics.Active;
            }
        }

        return active;
    }

    // private void OnF4Intent(InputAction.CallbackContext context) => ShowAll();

    private void OnF4Intent(InputAction.CallbackContext context)
    {
        var anyActive = AnyActive();

        if (anyActive)
        {
            HideAll();
        }
        else
        {
            ShowAll();
        }
    }

    private void HideAll()
    {
        if (scene.Attributes == null) return;
        scene.Attributes.SetView(AttributesUI.ViewEnum.None);

        if (scene.Actuators == null) return;
        scene.Actuators.Active = false;

        if (scene.Analytics == null) return;
        scene.Analytics.Active = false;
    }

    // private void OnF5Intent(InputAction.CallbackContext context) => HideAll();
}
