using System;

using UnityEngine;
using UnityEngine.InputSystem;

using UI;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] OnTrigger2DHandler groundTrigger;

    [Header("Audio")]
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip landClip;

    [Header("UI")]
    [SerializeField] AttributeUI isGroundedUI;
    [SerializeField] AttributeUI velocityXUI;
    [SerializeField] AttributeUI velocityYUI;
    [SerializeField] AttributeUI stateUI;

    [Header("Move")]
    [Range(10f, 50f)]
    [SerializeField] float moveSpeed = 25f;

    [Header("Jump")]
    [Range(0f, 250f)]
    [SerializeField] float minJumpSpeed = 250f;
    [Range(250f, 500f)]
    [SerializeField] float midJumpSpeed = 500f;
    [Range(500f, 750f)]
    [SerializeField] float maxJumpSpeed = 750f;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 100f;

    [Header("Move Stats")]
    [SerializeField] float moveXValue;
    [SerializeField] float trueMoveXValue;
    [SerializeField] float moveYValue;
    [SerializeField] float trueMoveYValue;

    [Header("Rigidbody")]
    [SerializeField] float velocityX;
    [SerializeField] float velocityY;

    [Header("Inferences")]
    [SerializeField] bool isGrounded;

    private InputSystem_Actions inputActions;
    private Rigidbody2D rigidBody;
    private AudioSource audioSource;
    private PlayerState state;

    private bool execRun, execJump/*, execPowerJump*/, execDash;
    private float jumpPressStartTime, jumpHeldDuration;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        // inputActions.Player.Move.performed += OnMoveIntent;
        // inputActions.Player.Jump.performed += OnJumpIntent;
        // inputActions.Player.PowerJump.performed += OnPowerJumpIntent;
        inputActions.Player.JumpPress.performed += OnJumpPressIntent;
        inputActions.Player.JumpRelease.performed += OnJumpReleaseIntent;
        inputActions.Player.Dash.performed += OnDashIntent;

        groundTrigger.Register(new OnTrigger2DHandler.Events
        {
            Gained = OnGround,
            Lost = OffGround
        });
    }

    void OnDisable() => inputActions.Disable();

    // private void OnMoveIntent(InputAction.CallbackContext context)
    // {
    //     var rawValue = inputActions.Player.Move.ReadValue<Vector2>();
    //     moveXValue = rawValue.x;
    //     moveYValue = rawValue.y;

    //     var value = rawValue * Time.deltaTime * moveSpeed;
    //     trueMoveXValue = value.x;
    //     trueMoveYValue = value.y;

    //     if (isGrounded && Mathf.Abs(trueMoveXValue) > Mathf.Epsilon)
    //     {
    //         execRun = true;
    //     }
    // }

    // private void OnJumpIntent(InputAction.CallbackContext context)
    // {
    //     Debug.Log("OnJumpIntent");

    //     if (isGrounded)
    //     {
    //         exeJump = true;
    //     }
    // }

    // private void OnPowerJumpIntent(InputAction.CallbackContext context)
    // {
    //     Debug.Log("OnPowerJumpIntent");

    //     if (isGrounded)
    //     {
    //         execPowerJump = true;
    //     }
    // }

    private void OnJumpPressIntent(InputAction.CallbackContext context)
    {
        // Debug.Log("OnJumpPressIntent");
        jumpPressStartTime = Time.time;
    }

    private void OnJumpReleaseIntent(InputAction.CallbackContext context)
    {
        // Debug.Log("OnJumpReleaseIntent");
        jumpHeldDuration = Time.time - jumpPressStartTime;
        execJump = true;
    }

    private void OnDashIntent(InputAction.CallbackContext context)
    {
        // Debug.Log("OnDashIntent");

        if (isGrounded)
        {
            execDash = true;
        }
    }

    private void EvalualRawIntents()
    {
        var rawValue = inputActions.Player.Move.ReadValue<Vector2>();
        moveXValue = rawValue.x;
        moveYValue = rawValue.y;

        var value = rawValue * Time.deltaTime * moveSpeed;
        trueMoveXValue = value.x;
        trueMoveYValue = value.y;

        OnRunIntent();
    }

    private void OnRunIntent()
    {
        if (isGrounded && Mathf.Abs(trueMoveXValue) > Mathf.Epsilon)
        {
            execRun = true;
        }
    }

    private void UpdateUI()
    {
        isGroundedUI.Value = isGrounded ? "True" : "False";

        velocityX = rigidBody.linearVelocityX;
        velocityY = rigidBody.linearVelocityY;

        velocityXUI.Value = velocityX.ToString();
        velocityYUI.Value = velocityY.ToString();

        stateUI.Value = state.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        EvalualRawIntents();
        UpdateUI();
    }

    private void ApplyRun()
    {
        rigidBody.linearVelocityX = trueMoveXValue * moveSpeed;
        execRun = false;
    }

    // private void ApplyJump()
    // {
    //     rigidBody.AddForce(Vector2.up * minJumpSpeed);
    //     execJump = false;
    // }

    // private void ApplyPowerJump()
    // {
    //     rigidBody.AddForce(Vector2.up * maxJumpSpeed);
    //     execPowerJump = false;
    // }

    private void ApplyJump(float heldDuration)
    {
        if (heldDuration > 0.4f)
        {
            rigidBody.AddForce(Vector2.up * maxJumpSpeed);
        }
        else if (heldDuration > 0.2f)
        {
            rigidBody.AddForce(Vector2.up * midJumpSpeed);
        }
        else
        {
            rigidBody.AddForce(Vector2.up * minJumpSpeed);
        }

        execJump = false;
    }

    private void ApplyDash()
    {
        var direction = Mathf.Sign(rigidBody.linearVelocityX);
        rigidBody.linearVelocityX = direction * dashSpeed;
        execDash = false;
    }

    public void OnGround(Collider2D collider)
    {
        audioSource.PlayOneShot(landClip, 1f);
        isGrounded = true;
    }

    public void OffGround(Collider2D collider)
    {
        audioSource.PlayOneShot(jumpClip, 1f);
        isGrounded = false;
    }

    void FixedUpdate()
    {
        state = PlayerState.Idle;

        if (rigidBody.linearVelocity.y > Mathf.Epsilon)
        {
            state = PlayerState.Jumping;
        }
        else if (rigidBody.linearVelocity.y < -Mathf.Epsilon)
        {
            state = PlayerState.Falling;
        }

        if(execRun)
        {
            ApplyRun();
            state = PlayerState.Running;
        }

        // if (execJump)
        // {
        //     ApplyJump();
        // }

        // if (execPowerJump)
        // {
        //     ApplyPowerJump();
        // }

        if (execJump)
        {
            ApplyJump(jumpHeldDuration);
        }

        
        if (execDash)
        {
            ApplyDash();
            state = PlayerState.Dashing;
        }
    }
}
