using UnityEngine;
using UnityEngine.U2D;

namespace Tests
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class SpriteShapeModifier : MonoBehaviour
    {
        [SerializeField] BaseEdgeModifier baseEdgeModifier;

        private SpriteShapeController spriteShapeController;
        private float topY;

        void Awake()
        {
            spriteShapeController = GetComponent<SpriteShapeController>();
            topY = GetSplinePosition(1).y;
        }

        public Vector3 GetSplinePosition(int index) => spriteShapeController.spline.GetPosition(index);

        public void ModifySplineYValue(int index, float value)
        {
            var position = GetSplinePosition(index);
            spriteShapeController.spline.SetPosition(index, new Vector3(position.x, value, position.z));
            baseEdgeModifier.SetTopOffset(value - topY);
        }
    }
}
