using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.Actions
{
    public class DashAction : Action
    {
        [Header("Configuration")]
        [Range(10f, 1500f)]
        [SerializeField] float speed = 1000f;
        [SerializeField] float duration = 0.05f;
        [SerializeField] AudioClip clip;

        private InputSystem_Actions inputActions;
        private Rigidbody2D rigidBody;
        private bool canDo/*, monitorDash*/;

        void Awake()
        {
            inputActions = Essentials.Transform().gameObject.GetComponent<CustomInputSystem>().InputActions;
            rigidBody = Essentials.RigidBody();
        }
        
        void OnEnable() => inputActions.Player.Dash.performed += OnIntent;

        void OnDisable() => inputActions.Player.Dash.performed -= OnIntent;

        private void OnIntent(InputAction.CallbackContext context)
        {
            if (!Essentials.PlayerStateActivation().CanDash) return;

            // if (Essentials.IsInputSuspended()) return;
            
            // if (!canExec) return;

            if (!Essentials.IsGrounded() || Essentials.PlayerState() != PlayerStateEnum.Moving || Essentials.IsDashing()) return;

            canDo = true;
        }

        private void Execute()
        {
            canDo = false;
            // monitorDash = true;

            Essentials.SetSuspendInput(true);
            Essentials.SetDashing(true);
            
            var direction = Mathf.Sign(rigidBody.linearVelocityX);
            rigidBody.linearVelocityX = direction * speed * Time.fixedDeltaTime;
            var audioSource = Essentials.AudioSource();
            audioSource.PlayOneShot(clip, 1f);

            StartCoroutine(Co_Dash());
        }

        private IEnumerator Co_Dash()
        {
            var elapsedTime = 0f;

            // var startPosition = Essentials.Transform().position.x;
            // Debug.Log($"Co_Dash Start Duration: {duration} Position: {startPosition} Velocity: {Mathf.Abs(rigidBody.linearVelocity.x)}");

            while (elapsedTime < duration && Mathf.Abs(rigidBody.linearVelocity.x) > AbstractedPlayer.MIN_REGISTERED_VALUE)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // var endPosition = Essentials.Transform().position.x;
            // Debug.Log($"Co_Dash End Velocity: {Mathf.Abs(rigidBody.linearVelocity.x)} Distance: {Mathf.Abs(endPosition - startPosition)} Elapsed Time: {elapsedTime}");

            Essentials.SetSuspendInput(false);
            Essentials.SetDashing(false);
        }

        // Update is called once per frame
        // void Update()
        // {
        //     canExec = !(Essentials.IsContactable() && Essentials.IsHolding());

        //     if (canExec)
        //     {
        //         EvaluateIntent();
        //     }
        // }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            // if (canExecute)
            // {
            //     ExecuteIntent();
            // }

            if (canDo)
            {
                Execute();
            }
        }
    }
}
