using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.State
{
    public class DashState : State
    {
        [Range(10f, 100f)]
        [SerializeField] float speed = 50f;
        [SerializeField] float duration = 0.05f;
        [SerializeField] AudioClip clip;

        private InputSystem_Actions inputActions;
        private bool execDash, canExec, monitorDash;

        void Awake() => inputActions = new InputSystem_Actions();
        
        void OnEnable()
        {
            inputActions.Enable();
            inputActions.Player.Dash.performed += OnDashIntent;
        }

        void OnDisable()
        {
            inputActions.Player.Dash.performed -= OnDashIntent;
            inputActions.Disable();
        }

        private void OnDashIntent(InputAction.CallbackContext context)
        {
            if (!canExec) return;

            if (Essentials.IsInputSuspended()) return;

            if (Essentials.PlayerState() == PlayerStateEnum.Running)
            {
                Essentials.SetSuspendInput(true);
                execDash = true;
            }
        }

        private void ApplyDash()
        {
            var direction = Mathf.Sign(Essentials.RigidBody().linearVelocityX);
            Essentials.RigidBody().linearVelocityX = direction * speed;
            Essentials.AudioSource().PlayOneShot(clip, 1f);

            execDash = false;
            monitorDash = true;
            Essentials.SetDashing(true);
        }

        private IEnumerator Co_Dash()
        {
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Essentials.SetDashing(false);
            Essentials.SetSuspendInput(false);
        }

        private void Evaluate()
        {
            if (monitorDash)
            {
                monitorDash = false;
                StartCoroutine(Co_Dash());
            }
        }

        // Update is called once per frame
        void Update()
        {
            canExec = !((Essentials.IsGrabbable() || Essentials.IsTraversable()) && Essentials.IsHolding());

            if (canExec)
            {
                Evaluate();
            }
        }

        void FixedUpdate()
        {
            if (!canExec) return;

            if (execDash)
            {
                ApplyDash();
            }
        }
    }
}
