using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExitPoint : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] PointID entryPointID;
    [SerializeField] PointFXType pointFXType;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            var sceneTransitionManager = FindFirstObjectByType<SceneTransitionManager>();

            if (sceneTransitionManager != null)
            {
                sceneTransitionManager.Transition(sceneName, entryPointID);
            }
        }
    }
}