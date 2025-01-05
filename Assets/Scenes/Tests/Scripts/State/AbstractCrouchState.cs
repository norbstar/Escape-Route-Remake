using System.Collections;

using UnityEngine;
using UnityEngine.U2D;

namespace Tests.State
{
    public abstract class AbstractCrouchState : State
    {
        [SerializeField] float deformationSpeed = 2.5f;

        private SpriteShapeController spriteShapeController;
        private float topY;

        protected IEnumerator Co_Crouch()
        {
            Essentials.SetCrouching(true);
            
            var minPosY = 0f;
            var posY = topY;
            float elapsedTime = 0f;

            var p1 = spriteShapeController.spline.GetPosition(1);
            var p2 = spriteShapeController.spline.GetPosition(2);

            while (posY > minPosY)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(topY, minPosY, elapsedTime * deformationSpeed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }
        }

        protected IEnumerator Co_Reset()
        {
            Essentials.SetCrouching(false);

            var minPosY = 0f;
            var posY = minPosY;
            float elapsedTime = 0f;

            var p1 = spriteShapeController.spline.GetPosition(1);
            var p2 = spriteShapeController.spline.GetPosition(2);

            while (posY < topY)
            {
                elapsedTime += Time.deltaTime;
                posY = Mathf.Lerp(minPosY, topY, elapsedTime * deformationSpeed);
                spriteShapeController.spline.SetPosition(1, new Vector3(p1.x, posY, p1.z));
                spriteShapeController.spline.SetPosition(2, new Vector3(p2.x, posY, p2.z));
                yield return null;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (spriteShapeController == null)
            {
                spriteShapeController = Essentials.SpriteShapeController();
                topY = spriteShapeController.spline.GetPosition(1).y;
            }
        }
    }
}
