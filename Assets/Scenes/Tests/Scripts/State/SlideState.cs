using System.Collections;

using UnityEngine;

namespace Tests.State
{
    public class SlideState : AbstractCrouchState
    {
        [Range(1f, 10f)]
        [SerializeField] float speed = 5f;

        private InputSystem_Actions inputActions;
        private bool execCrouch, execSlide, canExec, isSliding;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void Evaluate()
        {
            var crouchValue = inputActions.Player.Crouch.IsPressed();
            execCrouch = Essentials.IsGrounded() && !Essentials.IsCrouching() && crouchValue;

            execSlide = execCrouch && Mathf.Abs(Essentials.RigidBody().linearVelocityX) >= speed;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            canExec = !((Essentials.IsGrabbable() || Essentials.IsTraversable()) && Essentials.IsHolding());

            if (canExec)
            {
                canExec = !isSliding;
            }

            if (canExec)
            {
                // Debug.Log($"IsCrouching: {isCrouching}");
                Evaluate();
            }
                
            // Debug.Log($"SlideState CanExec: {canExec} CanSlide: {execSlide}");
        }

        private IEnumerator Co_Slide()
        {
            // Debug.Log($"Co_Slide Start");
            Essentials.SetSuspendInput(true);
            Essentials.SetSliding(true);
            isSliding = true;
            
            while (Essentials.RigidBody().linearVelocityX != 0f)
            {
                yield return null;
            }
            
            isSliding = false;
            Essentials.SetSliding(false);
            Essentials.SetSuspendInput(false);
            // Debug.Log($"Co_Slide End");
        }

        private void ApplyCrouch()
        {
            if (Essentials.IsCrouching()) return;
            // Debug.Log($"ApplyCrouch");
            StartCoroutine(Co_Crouch());
        }

        private void ApplySlide()
        {
            if (isSliding) return;
            // Debug.Log($"ApplySlide");
            StartCoroutine(Co_Slide());
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (execCrouch)
            {
                ApplyCrouch();
            }

            if (execSlide)
            {
                ApplySlide();
            }
        }
    }
}
