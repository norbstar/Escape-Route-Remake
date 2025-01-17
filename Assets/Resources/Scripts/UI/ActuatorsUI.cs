using UnityEngine;

using UI;

public class ActuatorsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Transform viewUI;

    [Header("UI Attributes")]
    [SerializeField] AttributeUI moveXUI;
    [SerializeField] AttributeUI moveYUI;
    [SerializeField] AttributeUI lookXUI;
    [SerializeField] AttributeUI lookYUI;
    [SerializeField] AttributeUI attackUI;
    [SerializeField] AttributeUI interactUI;
    [SerializeField] AttributeUI crouchUI;
    [SerializeField] AttributeUI jumpUI;
    [SerializeField] AttributeUI dashUI;
    [SerializeField] AttributeUI sprintUI;
    [SerializeField] AttributeUI holdUI;

    private InputSystem_Actions inputActions;

    void Awake() => inputActions = new InputSystem_Actions();

    void OnEnable() => inputActions.Enable();

    void OnDisable() => inputActions.Disable();

    private void UpdateUI()
    {     
        var moveValue = inputActions.Player.Move.ReadValue<Vector2>();

        if (moveXUI != null)
        {
            moveXUI.Value = moveValue.x.ToString("0.00");
        }

        if (moveYUI != null)
        {
            moveYUI.Value = moveValue.y.ToString("0.00");
        }

        var lookValue = inputActions.Player.Look.ReadValue<Vector2>();

        if (lookXUI != null)
        {
            lookXUI.Value = lookValue.x.ToString("0.00");
        }

        if (lookYUI != null)
        {
            lookYUI.Value = lookValue.y.ToString("0.00");
        }

        if (attackUI != null)
        {
            var attackValue = inputActions.Player.Attack.IsPressed();
            attackUI.Value = attackValue ? "True" : "False";
            attackUI.Color = attackValue ? Color.white : Color.grey;
        }

        if (interactUI != null)
        {
            var interactValue = inputActions.Player.Interact.IsPressed();
            interactUI.Value = interactValue ? "True" : "False";
            interactUI.Color = interactValue ? Color.white : Color.grey;
        }

        if (crouchUI != null)
        {
            var crouchValue = inputActions.Player.Crouch.IsPressed();
            crouchUI.Value = crouchValue ? "True" : "False";
            crouchUI.Color = crouchValue ? Color.white : Color.grey;
        }

        if (jumpUI != null)
        {
            var jumpValue = inputActions.Player.Jump.IsPressed();
            jumpUI.Value = jumpValue ? "True" : "False";
            jumpUI.Color = jumpValue ? Color.white : Color.grey;
        }

        if (dashUI != null)
        {
            var dashValue = inputActions.Player.Dash.IsPressed();
            dashUI.Value = dashValue ? "True" : "False";
            dashUI.Color = dashValue ? Color.white : Color.grey;
        }

        if (sprintUI != null)
        {
            var sprintValue = inputActions.Player.Sprint.IsPressed();
            sprintUI.Value = sprintValue ? "True" : "False";
            sprintUI.Color = sprintValue ? Color.white : Color.grey;
        }

        if (holdUI != null)
        {
            var holdValue = inputActions.Player.Hold.IsPressed();
            holdUI.Value = holdValue ? "True" : "False";
            holdUI.Color = holdValue ? Color.white : Color.grey;
        }
    }

    // Update is called once per frame
    void Update() => UpdateUI();

    public bool Active { get => viewUI.gameObject.activeSelf; set => viewUI.gameObject.SetActive(value); }

    public void FlipView() => Active = !viewUI.gameObject.activeSelf;
}
