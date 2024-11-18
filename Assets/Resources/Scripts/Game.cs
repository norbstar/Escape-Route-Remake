using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    void Awake() => inputActions = new InputSystem_Actions();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Game.Quit.performed += Callback_OnQuit;
    }

    void OnDisable() => inputActions.Disable();

    private void Callback_OnQuit(InputAction.CallbackContext context) => Application.Quit();

    // Update is called once per frame
    void Update()
    {
        
    }
}
