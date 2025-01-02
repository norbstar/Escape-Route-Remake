using UnityEngine;

namespace Tests
{
    public class LookAtTest : MonoBehaviour
    {
        [SerializeField] Transform target;
        
        // Update is called once per frame
        void Update()
        {
            // transform.LookAt(target.position, Vector3.left);
            // transform.right = target.position - transform.position;
            var direction = target.position - transform.position;
            var angle = ((Vector2) direction).ToAngle();
            transform.eulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}