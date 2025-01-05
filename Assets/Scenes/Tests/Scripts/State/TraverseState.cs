using UnityEngine;

namespace Tests.State
{
    public class TraverseState : State
    {
        [Range(5f, 25f)]
        [SerializeField] float speed = 15f;

        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private bool execTraverse, canExec;

        void Awake() => inputActions = new InputSystem_Actions();

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        private void Evaluate()
        {
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            Essentials.RigidBody().linearVelocity = Vector2.zero;

            if (moveValue != Vector2.zero)
            {
                ApplyTraverse();
            }
        }

        // Update is called once per frame
        void Update()
        {
            canExec = Essentials.IsTraversable() && Essentials.IsHolding();
            
            if (canExec)
            {
                Evaluate();
            }
        }

        private void ApplyTraverse()
        {
            Debug.Log($"ApplyTraverse");
            Essentials.Transform().Translate(new Vector3(moveValue.x, moveValue.y, 0f) * speed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (execTraverse)
            {
                ApplyTraverse();
            }
        }
    }
}
