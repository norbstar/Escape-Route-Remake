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

        private bool execDash, monitorDash;
        
        void OnEnable() => Essentials.InputActions().Player.Dash.performed += OnDashIntent;

        void OnDisable() => Essentials.InputActions().Player.Dash.performed -= OnDashIntent;

        private void OnDashIntent(InputAction.CallbackContext context)
        {
            if (Essentials.IsInputSuspended()) return;

            if (Essentials.PlayerState() == PlayerStateEnum.Running)
            {
                Essentials.SuspendInput(true);
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
            Essentials.Dashing(true);
        }

        private IEnumerator Co_Dash()
        {
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Essentials.Dashing(false);
            Essentials.SuspendInput(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (monitorDash)
            {
                monitorDash = false;
                StartCoroutine(Co_Dash());
            }
        }

        void FixedUpdate()
        {
            if (execDash)
            {
                ApplyDash();
            }
        }
    }
}
