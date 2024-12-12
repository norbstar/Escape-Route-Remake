using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tests
{
    [RequireComponent(typeof(AudioSource))]
    public class SweepingCamera : MonoBehaviour
    {
        public enum Mode
        {
            Sweep,
            Tracking
        }

        public enum Rotation
        {
            Clockwise,
            AntiClockwise
        }

        [Header("Components")]
        [SerializeField] Transform lightBase;
        [SerializeField] new Light2D light;

        [Header("Config")]
        [Range(0f, 90f)]
        [SerializeField] float leftSweepAngle;
        [Range(0f, 90f)]
        [SerializeField] float rightSweepAngle;
        [Range(0f, 50f)]
        [SerializeField] float sweepSpeed = 10f;
        [SerializeField] Rotation startRotation;

        [Header("Audio")]
        [SerializeField] AudioClip alarmClip;

        [Range(0, 10f)]
        [SerializeField] float maxRange = 5f;
        [SerializeField] Color sweepColor = Color.white;
        [SerializeField] Color trackingColor = Color.red;

        [Header("Analytics")]
        [SerializeField] float angle;
        [SerializeField] bool withinCone;
        [SerializeField] bool withinRange;
        [SerializeField] bool hasHit;
        [SerializeField] bool playerDetected;

        private Player player;
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private Mode mode, lastMode;
        private Rotation rotation;
        private int layerMask;

        void Awake()
        {
            player = FindAnyObjectByType<Player>();
            spriteRenderer = lightBase.GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            mode = lastMode = Mode.Sweep;
            layerMask = LayerMask.GetMask("Player");
            rotation = startRotation;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() => StartCoroutine(Co_Sweep());

        private bool IsPointWithinSensorCone(Vector3 point, out ConeData data)
        {
            withinCone = withinRange = false;

            var direction = point - light.transform.position;
            angle = Vector3.Angle(light.transform.up, direction);

            if (angle < light.pointLightOuterAngle / 2f)
            {
                withinCone = true;
            }

            withinRange = direction.magnitude <= maxRange;

            data = new ConeData();
            data.withinCone = withinCone;
            data.direction = (Vector2) direction;
            data.withinRange = withinRange;
            data.distance = direction.magnitude;

            return withinCone && withinRange;
        }

        private void UpdateColors()
        {
            switch (mode)
            {
                case Mode.Sweep:
                    spriteRenderer.color = light.color = sweepColor;
                    break;

                case Mode.Tracking:
                    spriteRenderer.color = light.color = trackingColor;
                    break;
            }
        }

        private IEnumerator Co_Sweep()
        {
            while (!playerDetected)
            {
                if (rotation == Rotation.Clockwise)
                {
                    light.transform.Rotate(new Vector3(0f, 0f, -1f), sweepSpeed * Time.deltaTime);

                    if (light.transform.eulerAngles.z < 180f - rightSweepAngle)
                    {
                        rotation = Rotation.AntiClockwise;
                    }
                }
                else
                {
                    light.transform.Rotate(new Vector3(0f, 0f, 1f), sweepSpeed * Time.deltaTime);

                    if (light.transform.eulerAngles.z > 180f + leftSweepAngle)
                    {
                        rotation = Rotation.Clockwise;
                    }
                }

                yield return null;
            }
        }

        private float Vector2ToAngle(Vector2 value)
        {
            var radians = Mathf.Atan2(value.y, value.x);
            return radians * (180f / Mathf.PI) - 90f;
        }

        private void OnSweep()
        {
            if (lastMode == Mode.Tracking)
            {
                audioSource.Stop();
                audioSource.loop = false;
                StartCoroutine(Co_Sweep());
            }
        }

        private void OnTracking(Vector3 direction)
        {
            var angle = Vector2ToAngle((Vector2) direction);
            light.transform.eulerAngles = new Vector3(0f, 0f, angle);

            if (lastMode == Mode.Sweep)
            {
                audioSource.loop = true;
                audioSource.clip = alarmClip;
                audioSource.Play();
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateColors();

            playerDetected = false;

            var inCone = IsPointWithinSensorCone(player.transform.position, out ConeData coneData);
            var direction = player.transform.position - light.transform.position;
            
            hasHit = ExtensionMethods.HasHit(light.transform.position, direction, Mathf.Infinity, layerMask, out ExtensionMethods.RaycastHitSet.Data hitData);
            playerDetected = inCone && hasHit && hitData.other.tag.Equals("Player");
            mode = playerDetected ? Mode.Tracking : Mode.Sweep;

            switch (mode)
            {
                case Mode.Sweep:
                    OnSweep();
                    break;

                case Mode.Tracking:
                    OnTracking(direction);
                    break;
            }

            lastMode = mode;
        }
    }
}