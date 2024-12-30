using UnityEngine;

using UI;

public class ActuatorsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Transform viewUI;

    [Header("UI Attributes")]
    [SerializeField] AttributeUI leftThumbstickXUI;
    [SerializeField] AttributeUI leftThumbstickYUI;
    [SerializeField] AttributeUI rightThumbstickXUI;
    [SerializeField] AttributeUI rightThumbstickYUI;

    private InputSystem_Actions inputActions;

    void Awake() => inputActions = new InputSystem_Actions();

    void OnEnable() => inputActions.Enable();

    void OnDisable() => inputActions.Disable();

    private void UpdateUI()
    {
        var moveValue = inputActions.Player.Move.ReadValue<Vector2>();

        if (leftThumbstickXUI != null)
        {
            leftThumbstickXUI.Value = moveValue.x.ToString("0.00");
        }

        if (leftThumbstickYUI != null)
        {
            leftThumbstickYUI.Value = moveValue.y.ToString("0.00");
        }

        var lookValue = inputActions.Player.Look.ReadValue<Vector2>();

        if (rightThumbstickXUI != null)
        {
            rightThumbstickXUI.Value = lookValue.x.ToString("0.00");
        }

        if (rightThumbstickYUI != null)
        {
            rightThumbstickYUI.Value = lookValue.y.ToString("0.00");
        }
    }

    // Update is called once per frame
    void Update() => UpdateUI();

    public void FlipView() => viewUI.gameObject.SetActive(!viewUI.gameObject.activeSelf);
}
