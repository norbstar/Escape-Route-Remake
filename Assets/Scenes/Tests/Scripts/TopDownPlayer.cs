using UnityEngine;

using Cinemachine;

namespace Tests
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TopDownPlayer : BasePlayerStub
    {
        [Header("Configuration")]
        [Range(1f, 10f)]
        [SerializeField] float speed = 10f;
        [Range(1f, 10f)]
        [SerializeField] float cameraTransitionSensitivity = 1f;
        // [SerializeField] bool applyDynamicPOV;

        // private new Camera camera;
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineComponentBase virtualCameraBase;
        private CinemachineFramingTransposer framingTransposer;
        private Rigidbody2D rigidBody;
        private InputSystem_Actions inputActions;
        private Vector2 moveValue;
        private float defaultCameraDistance;
        // private Coroutine coroutine;
        // private float defaultPOV;
        // private Vector2? lastLinearVelocity;

        void Awake()
        {
            // camera = FindFirstObjectByType<Camera>();
            virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
            virtualCameraBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            framingTransposer = (CinemachineFramingTransposer) virtualCameraBase;
            defaultCameraDistance = framingTransposer.m_CameraDistance;
            rigidBody = GetComponent<Rigidbody2D>();
            inputActions = new InputSystem_Actions();
            // defaultPOV = virtualCamera.m_Lens.FieldOfView;
        }

        void OnEnable() => inputActions.Enable();

        void OnDisable() => inputActions.Disable();

        // private IEnumerator Co_SmoothPOV()
        // {
        //     while (rigidBody.linearVelocity == lastLinearVelocity)
        //     {
        //         yield return null;
        //     }
        // }

        // Update is called once per frame
        // void Update()
        // {
        //     if (rigidBody.linearVelocity != lastLinearVelocity)
        //     {
        //         if (coroutine != null)
        //         {
        //             StopCoroutine(coroutine);
        //         }

        //         coroutine = StartCoroutine(Co_SmoothPOV());
        //     }
        // }

        // Update is called once per frame
        // void Update()
        // {
        //     if (!applyDynamicPOV) return;

        //     if (lastLinearVelocity != null)
        //     {
        //         var velocity = rigidBody.linearVelocity.magnitude;
        //         var targetPOV = defaultPOV + velocity;

        //         if (targetPOV != virtualCamera.m_Lens.FieldOfView)
        //         {
        //             var magnitudeDelta = velocity - lastLinearVelocity.Value.magnitude;
        //             virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera.m_Lens.FieldOfView + Mathf.Sign(magnitudeDelta) * cameraTransitionSpeed * Time.deltaTime, defaultPOV, targetPOV);
        //         }
        //     }

        //     lastLinearVelocity = rigidBody.linearVelocity;
        // }

        // Update is called once per frame
        void Update()
        {
            // if (!applyDynamicPOV) return;
            
            moveValue = inputActions.Player.Move.ReadValue<Vector2>();
            framingTransposer.m_CameraDistance = defaultCameraDistance + moveValue.magnitude * cameraTransitionSensitivity;
        }

        void FixedUpdate() => rigidBody.linearVelocity = moveValue * speed;
    }
}
