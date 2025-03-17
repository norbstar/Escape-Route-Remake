using System.Collections;

using UnityEngine;

namespace Tests.States
{
    public class SlideState : CrouchState
    {
        [Range(1f, 10f)]
        [SerializeField] float speed = 5f;

        private InputSystem_Actions inputActions;
        private bool execCrouch, execSlide, canExec, isSliding;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void EvaluateIntent()
        {
            var crouchValue = inputActions.Player.Crouch.IsPressed();
            execCrouch = Essentials.IsGrounded() && !Essentials.IsCrouching() && crouchValue;

            execSlide = execCrouch && Mathf.Abs(Essentials.RigidBody().linearVelocityX) >= speed;
        }

        // Update is called once per frame
        void Update()
        {
            canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            if (canExec)
            {
                canExec = !isSliding;
            }

            if (canExec)
            {
                EvaluateIntent();
            }
        }

        private IEnumerator Co_Slide()
        {
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
        }

        private void ExecuteCrouch() => Crouch();

        private void ExecuteSlide()
        {
            if (isSliding) return;
            StartCoroutine(Co_Slide());
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (execCrouch)
            {
                ExecuteCrouch();
            }

            if (execSlide)
            {
                ExecuteSlide();
            }
        }
    }
}
