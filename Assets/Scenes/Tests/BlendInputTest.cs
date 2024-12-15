using UnityEngine;

using UI;
using UnityEngine.UI;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BlendInputTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Slider blendSpeedUI;

        [Header("Movement")]
        [Range(0f, 100f)]
        [SerializeField] float moveSpeed = 10f;

        [Header("Blending")]
        [Range(0f, 1f)]
        [SerializeField] float blendSpeed = 1f;
        // [SerializeField] bool enableBlendSpeedRelativity = true;

        [Header("UI")]
        [SerializeField] AttributeUI signUI;
        [SerializeField] AttributeUI stepSizeUI;
        [SerializeField] AttributeUI blendValueUI;
        [SerializeField] AttributeUI targetValueUI;

        [Header("Stats")]
        [SerializeField] Vector2 moveValue;
        [SerializeField] float moveXValue;
        [SerializeField] float blendValue;

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
            targetValueUI.Value = moveXValue.ToString("0.00");

            if (Mathf.Abs(moveXValue) != 0f)
            {
                OnMoveXIntent();
            }
            else
            {
                rigidBody.linearVelocityX = 0f;
                blendValue = 0f;
                blendValueUI.Value = blendValue.ToString("0.00");
            }
        }

        private void OnMoveXIntent() => execRun = true;

        private void ApplyRun()
        {
            var sign = Mathf.Sign(moveXValue);
            signUI.Value = sign.ToString();
            var blendSpeed = this.blendSpeed = blendSpeedUI.value;
            var stepSize = blendSpeed * Time.fixedDeltaTime;
            // var stepSize = enableBlendSpeedRelativity ? 0f : blendSpeed * Time.fixedDeltaTime;
            stepSizeUI.Value = stepSize.ToString();

            if (sign > 0)
            {
                blendValue = blendValue + stepSize > moveXValue ? moveXValue : blendValue + stepSize;
            }
            else
            {
                blendValue = blendValue - stepSize < moveXValue ? moveXValue : blendValue - stepSize;
            }

            blendValueUI.Value = blendValue.ToString();
            rigidBody.linearVelocityX = blendValue * moveSpeed;
            execRun = false;
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