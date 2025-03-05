using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
{
    private SceneWipeUIManager manager;

    private IEnumerator StreamSceneCoroutine(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            var sceneLoaderAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!sceneLoaderAsync.isDone) yield return null;
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, float fadeSpeed)
    {
        if (manager == null)
        {
            manager = (SceneWipeUIManager) SceneWipeUIManager.Instance;
        }

        yield return manager.FadeInCoroutine(fadeSpeed);
        yield return StartCoroutine(StreamSceneCoroutine(sceneName));
        yield return manager.FadeOutCoroutine(fadeSpeed);
    }

    public void LoadScene(string sceneName, float fadeSpeed = 5f) => StartCoroutine(LoadSceneCoroutine(sceneName, fadeSpeed));
}
