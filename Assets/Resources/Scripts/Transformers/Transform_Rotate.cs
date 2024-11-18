using UnityEngine;

namespace Transformers
{
    public class Transform_Rotate : MonoBehaviour
    {
        [SerializeField]
        public Vector3 rotationSpeed;

        // Update is called once per frame
        void Update() => transform.Rotate(new Vector3(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z) * Time.deltaTime);
    }
}