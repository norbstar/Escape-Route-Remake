using UnityEngine;
using UnityEngine.InputSystem;

using Tests;

[RequireComponent(typeof(CanvasManager))]
public class DebugOverlayUI : MonoBehaviour
{
    private CanvasManager canvasManager;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        canvasManager = GetComponent<CanvasManager>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Game.F1.performed += OnF1Intent;
        inputActions.Game.F2.performed += OnF2Intent;
        inputActions.Game.F3.performed += OnF3Intent;
        inputActions.Game.F4.performed += OnF4Intent;
    }

    void OnDisable() => inputActions.Disable();

    private void OnF1Intent(InputAction.CallbackContext context)
    {
        if (canvasManager.Attributes == null) return;
        canvasManager.Attributes.CycleView();
    }

    private void OnF2Intent(InputAction.CallbackContext context)
    {
        if (canvasManager.Actuators == null) return;
        canvasManager.Actuators.FlipView();
    }

    private void OnF3Intent(InputAction.CallbackContext context)
    {
        if (canvasManager.Analytics == null) return;
        canvasManager.Analytics.FlipView();
    }

    private void ShowAll()
    {
        if (canvasManager.Attributes == null) return;
        canvasManager.Attributes.SetView(AttributesUI.ViewEnum.All);

        if (canvasManager.Actuators == null) return;
        canvasManager.Actuators.Active = true;

        if (canvasManager.Analytics == null) return;
        canvasManager.Analytics.Active = true;
    }

    private bool AnyActive()
    {
        bool active = false;

        if (canvasManager.Attributes != null)
        {
            active = canvasManager.Attributes.Active;
        }

        if (!active)
        {
            if (canvasManager.Actuators != null)
            {
                active = canvasManager.Actuators.Active;
            }
        }

        if (!active)
        {
            if (canvasManager.Analytics != null)
            {
                active = canvasManager.Analytics.Active;
            }
        }

        return active;
    }

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
        if (canvasManager.Attributes == null) return;
        canvasManager.Attributes.SetView(AttributesUI.ViewEnum.None);

        if (canvasManager.Actuators == null) return;
        canvasManager.Actuators.Active = false;

        if (canvasManager.Analytics == null) return;
        canvasManager.Analytics.Active = false;
    }
}
