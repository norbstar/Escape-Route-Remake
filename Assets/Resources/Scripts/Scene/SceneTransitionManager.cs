using UnityEngine;
using UnityEngine.SceneManagement;

using Tests;

namespace Scene
{
    public class SceneTransitionManager : MonoBehaviour
    {
        private void LoadScene(string sceneName)
        {
            var instance = (SceneLoader) SceneLoader.Instance;
            instance.LoadScene(sceneName);
        }

        public virtual void Transition(string sceneName, PointID entryPointID)
        {
            var activeScene = SceneManager.GetActiveScene();
            var activeSceneName = activeScene.name;

            var basePlayer = (BasePlayer) BasePlayer.Instance;

            if (basePlayer == null) return;

            var rigidBody = basePlayer.GetComponent<Rigidbody2D>();
            rigidBody.linearVelocity = Vector3.zero;

            basePlayer.Deactivate();

            if (activeSceneName.Equals(sceneName))
            {
                var entryPoints = FindObjectsByType<EntryPoint>(FindObjectsSortMode.None);

                foreach (var entryPoint in entryPoints)
                {
                    if (entryPointID == entryPoint.EntryPointID)
                    {
                        basePlayer.gameObject.transform.position = entryPoint.transform.position;
                        basePlayer.Activate();
                        return;
                    }
                }
            }

            var sceneSetup = (SceneSetup) SceneSetup.Instance;
            sceneSetup.EntryPointID = entryPointID;
            
            LoadScene(sceneName);
        }
    }
}
