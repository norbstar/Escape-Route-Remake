using UI;
using UnityEngine;
// using UnityEngine.InputSystem;

namespace Tests
{
    public class InputActionMapping : MonoBehaviour
    {
#if false
        // [SerializeField] InputAction moveX;
        // [SerializeField] InputAction moveY;

        [SerializeField] float moveSpeed = 1f;

        [Header("Move Stats")]
        [SerializeField] float moveXValue;
        [SerializeField] float regulatedMoveXValue;
        [SerializeField] float moveYValue;
        [SerializeField] float regulatedMoveYValue;

        // [Header("Attack Stats")]
        // [SerializeField] bool attack;

        private InputSystem_Actions inputActions;

        void Awake() => inputActions = new InputSystem_Actions();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // void OnEnable()
        // {
        //     moveX.Enable();
        //     moveX.performed += Callback_OnMoveX;

        //     moveY.Enable();
        //     moveY.performed += Callback_OnMoveY;
        // }

        void OnEnable()
        {
            inputActions.Enable();

            inputActions.Player.Attack.performed += Callback_OnAttack;
            inputActions.Player.Interact.performed += Callback_OnInteract;
            inputActions.Player.Crouch.performed += Callback_OnCrouch;
            inputActions.Player.Jump.performed += Callback_OnJump;
            inputActions.Player.Sprint.performed += Callback_OnSprint;
        }

        // void OnDisable()
        // {
        //     moveX.Disable();
        //     moveY.Disable();
        // }

        void OnDisable() => inputActions.Disable();

        // private void Callback_OnMoveX(InputAction.CallbackContext context)
        // {
        //     moveXValue = context.ReadValue<float>();
        //     var regulatedValue = moveXValue * Time.deltaTime * moveSpeed;

        //     regulatedMoveXValue = regulatedValue;
        // }

        // private void Callback_OnMoveY(InputAction.CallbackContext context)
        // {
        //     moveYValue = context.ReadValue<float>();
        //     var regulatedValue = moveYValue * Time.deltaTime * moveSpeed;

        //     regulatedMoveYValue = regulatedValue;
        // }

        private void ScanMoveValues()
        {
            var rawValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = rawValue.x;
            moveYValue = rawValue.y;

            var regulatedValue = rawValue * Time.deltaTime * moveSpeed;
            regulatedMoveXValue = regulatedValue.x;
            regulatedMoveYValue = regulatedValue.y;
        }

        private void Callback_OnAttack(InputAction.CallbackContext context) => Debug.Log("Attack");

        private void Callback_OnInteract(InputAction.CallbackContext context) => Debug.Log("Interact");

        private void Callback_OnCrouch(InputAction.CallbackContext context) => Debug.Log("Crouch");

        private void Callback_OnJump(InputAction.CallbackContext context) => Debug.Log("Jump");

        private void Callback_OnSprint(InputAction.CallbackContext context) => Debug.Log("Sprint");

        // private void ScanAttackValue()
        // {
        //     var rawValue = inputActions.Player.Attack.ReadValue<bool>();
        //     attack = rawValue;
        // }

        // Update is called once per frame
        void Update()
        {
            // attack = false;

            ScanMoveValues();
            // ScanAttackValue();
        }
#endif

        [Header("UI")]
        [SerializeField] EnergyBarUI energyBar;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        public EnergyBarUI EnergyBarUI => energyBar;

        // Update is called once per frame
        void Update()
        {

        }
    }
}