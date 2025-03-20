using UnityEngine;

namespace Tests.Actions
{
    public class MoveAction : Action
    {
        [Header("Configuration")]
        [Range(1f, 400f)]
        [SerializeField] float speed = 200f;
        // [SerializeField] bool dampedSpeed;

        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        private Vector2 moveValue;
        private bool canDo;

        void Awake()
        {
            inputActions = Essentials.Transform().gameObject.GetComponent<CustomInputSystem>().InputActions;
            rigidBody = Essentials.RigidBody();
        }

        private void Evaluate()
        {
            if (!Essentials.PlayerStateActivation().CanMove) return;

            // if (Essentials.IsInputSuspended()) return;

            // var canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

            // if (!canExec) return;

            // var canExec = !Essentials.IsCrouching();

            // if (!canExec) return;

            // moveValue = inputActions.Player.Move.ReadValue<Vector2>();

            // if (!Essentials.IsGrounded() || Essentials.IsCrouching() || Mathf.Abs(moveValue.x) == AbstractedStatePlayer.MIN_REGISTERED_VALUE) return;

            if (!Essentials.IsGrounded() || Essentials.IsDashing()) return;

            // moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            // if (Mathf.Abs(moveValue.x) < AbstractedStatePlayer.MIN_REGISTERED_VALUE) return;

            canDo = true;
        }

        private void Execute()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            rigidBody.linearVelocityX = moveValue.x * speed * Time.fixedDeltaTime;

            canDo = false;
        }

        public override void FixedUpdate()
        {
            // if (!Essentials.PlayerStateActivation().CanMove) return;
            
            base.FixedUpdate();

            // if (canExecute)
            // {
            //     ExecuteIntent();
            // }
            // else
            // {
            //     Essentials.RigidBody().linearVelocityX = 0f;
            // }

            Evaluate();

            if (canDo)
            {
                Execute();
            }
            
            // if (execMove)
            // {
            //     ExecuteIntent();
            // }
            // else if (dampedSpeed && Essentials.IsGrounded() && !Essentials.IsDashing())
            // {
            //     Essentials.RigidBody().linearVelocityX = 0f;
            // }
        }
    }
}
