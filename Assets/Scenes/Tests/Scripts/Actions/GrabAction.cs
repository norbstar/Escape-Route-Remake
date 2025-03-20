using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.Actions
{
    public class GrabAction : Action
    {
        [Header("Configuration")]
        [Range(400f, 800f)]
        [SerializeField] float leapForce = 600f;
        [SerializeField] AudioClip leapClip;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execLeap, canExec;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable()
        {
            inputActions.Enable();
            inputActions.Player.JumpPress.performed += OnJumpPressIntent;
        }

        void OnDisable()
        {
            inputActions.Player.JumpPress.performed -= OnJumpPressIntent;
            inputActions.Disable();
        }

        private void OnJumpPressIntent(InputAction.CallbackContext context)
        {
            if (!canExec) return;
            execLeap = true;
        }
        
        private void Evaluate()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            Essentials.RigidBody().linearVelocity = Vector2.zero;
        }

        // Update is called once per frame
        void Update()
        {
            canExec = Essentials.IsGrabbable() && Essentials.IsHolding();
            Essentials.ShowArrow(canExec);

            if (canExec)
            {
                Evaluate();
            }
        }

        private void ApplyLeap()
        {
            var gameObject = Essentials.GrabbableGameObject();

            if (gameObject.TryGetComponent<Grabbable>(out var grabbable))
            {
                grabbable.DisableColliderTemporarily();
            }

            Essentials.RigidBody().AddForce(moveValue * leapForce);
            Essentials.AudioSource().PlayOneShot(leapClip, 1f);
            execLeap = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!canExec) return;

            // if (Essentials.IsInputSuspended()) return;

            if (execLeap)
            {
                ApplyLeap();
            }
        }
    }
}
