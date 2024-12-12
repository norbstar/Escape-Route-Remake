using Unity.VisualScripting;
using UnityEngine;

namespace Tests
{
    public class LookAtTest : MonoBehaviour
    {
        [SerializeField] Transform target;
        
        private float Vector2ToAngle(Vector2 value)
        {
            var radians = Mathf.Atan2(value.y, value.x);
            return radians * (180f / Mathf.PI) - 90f;
        }

        // Update is called once per frame
        void Update()
        {
            // transform.LookAt(target.position, Vector3.left);
            // transform.right = target.position - transform.position;
            var direction = target.position - transform.position;
            var angle = Vector2ToAngle((Vector2) direction);
            transform.eulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}