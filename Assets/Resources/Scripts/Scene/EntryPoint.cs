using UnityEngine;

using Tests;

namespace Scene
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] PointID entryPointID;
        // [SerializeField] PointFXType pointFXType;

        public PointID EntryPointID => entryPointID;

        // Start is called before the first frame update
        void Start()
        {
            var basePlayer = (BasePlayer) BasePlayer.Instance;

            if (basePlayer == null) return;

            var sceneSetup = (SceneSetup) SceneSetup.Instance;

            if (sceneSetup == null || entryPointID != sceneSetup.EntryPointID) return;

            basePlayer.gameObject.transform.position = transform.position;
            basePlayer.Activate();
        }
    }
}