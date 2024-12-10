using UnityEngine;

using UI;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RelativeInputSensitivityTest : MonoBehaviour
    {
        [Header("Move")]
        [Range(0f, 20f)]
        [SerializeField] float moveSpeed = 15f;
        
        [Header("Sensitivity")]
        [SerializeField] AnimationCurve sensitivityProfile;

        [Header("UI")]
        [SerializeField] AttributeUI inputSensitivityUI;
        [SerializeField] AttributeUI relativeMoveSpeedUI;
        [SerializeField] AttributeUI velocityXUI;

        [Header("Stats")]
        [SerializeField] Vector2 moveValue;
        [SerializeField] float moveXValue;
        [SerializeField] float inputSensitivity;
        [SerializeField] float relativeMoveSpeed;

        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        private bool execRun;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            inputActions = new InputSystem_Actions();
        }

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void ScanRawIntents()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            moveXValue = moveValue.x;

            if (Mathf.Abs(moveXValue) != 0f)
            {
                OnMoveXIntent();
            }
            else
            {
                rigidBody.linearVelocityX = 0f;
            }
        }

        private void OnMoveXIntent() => execRun = true;

        private void ApplyRun()
        {
            // var normalised = Mathf.Abs(rigidBody.linearVelocityX) > 0 ? Mathf.Abs(rigidBody.linearVelocityX) / moveSpeed : 0f;
            // inputSensitivity = sensitivityProfile.Evaluate(normalised);
            // relativeMoveSpeed = moveXValue * moveSpeed;
            // rigidBody.linearVelocityX = relativeMoveSpeed * inputSensitivity;

            inputSensitivity = sensitivityProfile.Evaluate(Mathf.Abs(moveXValue));
            relativeMoveSpeed = moveSpeed * Mathf.Sign(moveXValue) * inputSensitivity;
            rigidBody.linearVelocityX = relativeMoveSpeed;
            execRun = false;
        }   

        void Update()
        {
            inputSensitivityUI.Value = inputSensitivity.ToString("0.00");
            relativeMoveSpeedUI.Value = relativeMoveSpeed.ToString("0.00");
            velocityXUI.Value = rigidBody.linearVelocityX.ToString("0.00");
        }

        void FixedUpdate()
        {
            ScanRawIntents();

            if (execRun)
            {
                ApplyRun();
            }
        }
    }
}