using System.Collections;

using UnityEngine;
using UnityEngine.U2D;

namespace Tests.State
{
    public class FallState : State
    {
        [SerializeField] AudioClip landClip;
        [SerializeField] float deformationSpeed = 2.5f;

        public static float SQUISH_PER_POINT = 0.1f;

        private SpriteShapeController spriteShapeController;
        private float lastLinearVelocityY;

        void Awake() => spriteShapeController = Essentials.SpriteShapeController();

        private IEnumerator Co_Squish(float squishFactor)
        {
            var minPosY = 0.425f - 0.425f * squishFactor;
            var posY = 0.425f;
            float elapsedTime = 0f;

            var p1 = spriteShapeController.spline.GetPosition(1);
            var p2 = spriteShapeController.spline.GetPosition(2);

            while (posY > minPosY)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(0.425f, minPosY, elapsedTime * deformationSpeed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }

            posY = minPosY;
            elapsedTime = 0f;

            while (posY < 0.425f)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(minPosY, 0.425f, elapsedTime * deformationSpeed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
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
        }

        void FixedUpdate() => lastLinearVelocityY = Essentials.RigidBody().linearVelocityY;
    }
}
