using System.Collections;

using UnityEngine;

namespace Tests.Events
{
    public class FallEvent : Event
    {
        [Header("Configuration")]
        [SerializeField] AudioClip landClip;
        [SerializeField] float deformationSpeed = 12f;

        public static float SQUISH_PER_POINT = 0.1f;

        public delegate void OnLanded(FallEvent instance);

        private SpriteShapeModifier spriteShapeModifier;
        private OnLanded onLanded;
        private float lastLinearVelocityY;
        private float topY;

        public void Subscribe(OnLanded onLanded) => this.onLanded += onLanded;

        public void Unsubscribe(OnLanded onLanded) => this.onLanded -= onLanded;

        private IEnumerator Co_Squish(float squishFactor)
        {
            var minPosY = topY - topY * squishFactor;
            var posY = topY;
            float elapsedTime = 0f;

            while (posY > minPosY)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(topY, minPosY, elapsedTime * deformationSpeed);
                spriteShapeModifier.ModifySplineYValue(1, posY);
                spriteShapeModifier.ModifySplineYValue(2, posY);
                yield return null;
            }

            posY = minPosY;
            elapsedTime = 0f;

            while (posY < topY)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(minPosY, topY, elapsedTime * deformationSpeed);
                spriteShapeModifier.ModifySplineYValue(1, posY);
                spriteShapeModifier.ModifySplineYValue(2, posY);
                yield return null;
            }
        }

        public override void OnGrounded()
        {
            Essentials.AudioSource().PlayOneShot(landClip, 1f);

            var squishPoints = 0f - lastLinearVelocityY;

            if (squishPoints > 0f)
            {
                StartCoroutine(Co_Squish(Mathf.Clamp(SQUISH_PER_POINT * squishPoints, 0f, 1f)));
            }

            onLanded?.Invoke(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (spriteShapeModifier == null)
            {
                spriteShapeModifier = Essentials.SpriteShapeModifier();
                topY = spriteShapeModifier.GetSplinePosition(1).y;
            }
        }

        void FixedUpdate() => lastLinearVelocityY = Essentials.RigidBody().linearVelocityY;
    }
}
