using UnityEngine;

namespace Tests
{
    public class InputSuspendedTest : MonoBehaviour
    {
        [SerializeField] bool enableInputSystem;

        private CustomInputSystem inputSystem;
        private InputSystem_Actions inputActions;

        void Awake()
        {
            var basePlayer = FindAnyObjectByType<BasePlayer>();
            inputSystem = basePlayer.GetComponent<CustomInputSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            inputSystem.Enable(enableInputSystem);
            
            if (inputActions == null)
            {
                inputActions = inputSystem.InputActions;
            }
            
            var moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            Debug.Log($"MoveValue: {moveValue}");
        }
    }
}
