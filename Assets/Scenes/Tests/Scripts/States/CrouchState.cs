using System.Collections;

using UnityEngine;

namespace Tests.States
{
    public abstract class CrouchState : State
    {
        [Header("Configuration")]
        [SerializeField] float deformationSpeed = 12f;

        private SpriteShapeModifier spriteShapeModifier;
        private Coroutine coroutine;
        private float topY;

        private IEnumerator Co_Crouch()
        {
            Essentials.SetCrouching(true);
            
            var minPosY = 0f;
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
        }

        protected void Crouch()
        {
            if (Essentials.IsCrouching()) return;
            
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(Co_Crouch());
        }

        private IEnumerator Co_Reset()
        {
            Essentials.SetCrouching(false);

            var minPosY = 0f;
            var posY = minPosY;
            float elapsedTime = 0f;

            while (posY < topY)
            {
                if (!Essentials.IsBlockedTop())
                {
                    elapsedTime += Time.deltaTime;
                    posY = Mathf.Lerp(minPosY, topY, elapsedTime * deformationSpeed);
                    spriteShapeModifier.ModifySplineYValue(1, posY);
                    spriteShapeModifier.ModifySplineYValue(2, posY);
                }

                yield return null;
            }
        }

        protected void Reset()
        {
            if (!Essentials.IsCrouching()) return;
            
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            
            coroutine = StartCoroutine(Co_Reset());
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
    }
}
