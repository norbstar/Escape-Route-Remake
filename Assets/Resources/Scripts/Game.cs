using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    void Awake() => inputActions = new InputSystem_Actions();

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Game.Quit.performed += Callback_OnQuit;
    }

    void OnDisable() => inputActions.Disable();

    private void Callback_OnQuit(InputAction.CallbackContext context) => Application.Quit();
}
