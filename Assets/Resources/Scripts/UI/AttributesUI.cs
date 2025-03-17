using UnityEngine;

using UI;

using Tests;

public class AttributesUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] UnityEngine.Transform viewUI;
    [SerializeField] UnityEngine.Transform boolAttributesUI;
    [SerializeField] UnityEngine.Transform numericalAttributesUI;

    [Header("UI Attributes")]
    [SerializeField] AttributeUI isBlockedTopUI;
    [SerializeField] AttributeUI isBlockedRightUI;
    [SerializeField] AttributeUI isGroundedUI;
    [SerializeField] AttributeUI isBlockedLeftUI;
    [SerializeField] AttributeUI isDashingUI;
    [SerializeField] AttributeUI isGrabbableUI;
    [SerializeField] AttributeUI isTraversableUI;
    [SerializeField] AttributeUI isCrouchingUI;
    [SerializeField] AttributeUI isGravityEnabledUI;
    [SerializeField] AttributeUI positionUI;
    [SerializeField] AttributeUI moveBearingUI;
    [SerializeField] AttributeUI moveAngleUI;
    [SerializeField] AttributeUI lookBearingUI;
    [SerializeField] AttributeUI lookAngleUI;
    [SerializeField] AttributeUI velocityXUI;
    [SerializeField] AttributeUI velocityYUI;
    [SerializeField] AttributeUI stateUI;

    public enum ViewEnum
    {
        None,
        Booleans,
        Values,
        All
    }

    private InputSystem_Actions inputActions;
    private PlayerEssentials essentials;
    private ViewEnum view;

    void Awake() => inputActions = new InputSystem_Actions();

    void OnEnable() => inputActions.Enable();

    void OnDisable() => inputActions.Disable();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => SetView(ViewEnum.None);

    private void UpdateUI()
    {
        if (essentials == null)
        {
            var basePlayer = FindFirstObjectByType<BasePlayer>();

            if (basePlayer != null)
            {
                essentials = basePlayer;
            }
        }

        if (essentials == null) return;

        if (isBlockedTopUI != null)
        {
            var isBlockedTop = essentials.IsBlockedTop();
            isBlockedTopUI.Value = isBlockedTop ? "True" : "False";
            isBlockedTopUI.Color = isBlockedTop ? Color.white : Color.grey;
        }
        
        if (isBlockedRightUI != null)
        {
            var isBlockedRight = essentials.IsBlockedRight();
            isBlockedRightUI.Value = isBlockedRight ? "True" : "False";
            isBlockedRightUI.Color = isBlockedRight ? Color.white : Color.grey;
        }
        
        if (isGroundedUI != null)
        {
            var isGrounded = essentials.IsGrounded();
            isGroundedUI.Value = isGrounded ? "True" : "False";
            isGroundedUI.Color = isGrounded ? Color.white : Color.grey;
        }
        
        if (isBlockedLeftUI != null)
        {
            var isBlockedLeft = essentials.IsBlockedLeft();
            isBlockedLeftUI.Value = isBlockedLeft ? "True" : "False";
            isBlockedLeftUI.Color = isBlockedLeft ? Color.white : Color.grey;
        }

        if (isDashingUI != null)
        {
            var isDashing = essentials.IsDashing();
            isDashingUI.Value = isDashing ? "True" : "False";
            isDashingUI.Color = isDashing ? Color.white : Color.grey;
        }

        if (isGrabbableUI != null)
        {
            var isGrabbable = essentials.IsGrabbable();
            isGrabbableUI.Value = isGrabbable ? "True" : "False";
            isGrabbableUI.Color = isGrabbable ? Color.white : Color.grey;
        }

        if (isTraversableUI != null)
        {
            var isTraversable = essentials.IsTraversable();
            isTraversableUI.Value = isTraversable ? "True" : "False";
            isTraversableUI.Color = isTraversable ? Color.white : Color.grey;
        }

        if (isCrouchingUI != null)
        {
            var isCrouching = essentials.IsCrouching();
            isCrouchingUI.Value = isCrouching ? "True" : "False";
            isCrouchingUI.Color = isCrouching ? Color.white : Color.grey;
        }

        if (isGravityEnabledUI != null)
        {
            var isGravityEnabled = essentials.IsGravityEnabled();
            isGravityEnabledUI.Value = isGravityEnabled ? "True" : "False";
            isGravityEnabledUI.Color = isGravityEnabled ? Color.white : Color.grey;
        }

        if (positionUI != null)
        {
            var position = essentials.Transform().position;
            positionUI.Value = $"[{position.x.ToString("0.00")}, {position.y.ToString("0.00")}]";
        }

        var moveValue = inputActions.Player.Move.ReadValue<Vector2>();

        if (moveBearingUI != null)
        {
            moveBearingUI.Value = $"[{moveValue.x.ToString("0.00")}, {moveValue.y.ToString("0.00")}]";
        }
        
        var moveAngle = moveValue.ToAngle360();

        if (moveAngleUI != null)
        {
            moveAngleUI.Value = moveAngle.ToString("0.00");
        }

        var lookValue = inputActions.Player.Look.ReadValue<Vector2>();

        if (lookBearingUI != null)
        {
            lookBearingUI.Value = $"[{lookValue.x.ToString("0.00")}, {lookValue.y.ToString("0.00")}]";
        }
        
        var lookAngle = lookValue.ToAngle360();

        if (lookAngleUI != null)
        {
            lookAngleUI.Value = lookAngle.ToString("0.00");
        }

        var velocityX = essentials.RigidBody().linearVelocityX;

        if (velocityXUI != null)
        {
            velocityXUI.Value = velocityX.ToString("0.00");
        }

        var velocityY = essentials.RigidBody().linearVelocityY;
        
        if (velocityYUI != null)
        {
            velocityYUI.Value = velocityY.ToString("0.00");
        }

        if (stateUI != null)
        {
            stateUI.Value = essentials.PlayerState().ToString();
        }
    }

    // Update is called once per frame
    void Update() => UpdateUI();

    public bool Active { get => viewUI.gameObject.activeSelf; set => viewUI.gameObject.SetActive(value); }

    public void SetView(ViewEnum view)
    {
        switch (view)
        {
            case ViewEnum.None:
                boolAttributesUI.gameObject.SetActive(false);
                numericalAttributesUI.gameObject.SetActive(false);
                break;

            case ViewEnum.Booleans:
                numericalAttributesUI.gameObject.SetActive(false);
                boolAttributesUI.gameObject.SetActive(true);
                break;

            case ViewEnum.Values:
                boolAttributesUI.gameObject.SetActive(false);
                numericalAttributesUI.gameObject.SetActive(true);
                break;

            case ViewEnum.All:
                boolAttributesUI.gameObject.SetActive(true);
                numericalAttributesUI.gameObject.SetActive(true);
                break;
        }

        Active = boolAttributesUI.gameObject.activeSelf || numericalAttributesUI.gameObject.activeSelf;
        
        this.view = view;
    }

    public void CycleView()
    {
        var nextView = view;

        switch (view)
        {
            case ViewEnum.None:
                nextView = ViewEnum.Booleans;
                break;

            case ViewEnum.Booleans:
                nextView = ViewEnum.Values;
                break;

            case ViewEnum.Values:
                nextView = ViewEnum.All;
                break;

            case ViewEnum.All:
                nextView = ViewEnum.None;
                break;
        }

        SetView(nextView);
    }
}
