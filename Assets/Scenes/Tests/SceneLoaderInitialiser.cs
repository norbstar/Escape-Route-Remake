// using System.Collections;

using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class SceneLoaderInitialiser : MonoBehaviour
{
    [SerializeField] string sceneName;

    private SceneLoader sceneLoader;

    void Awake() => sceneLoader = GetComponent<SceneLoader>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneLoader.LoadScene(sceneName);
        enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start() => StartCoroutine(LateStart());

    // private IEnumerator LateStart()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     sceneLoader.LoadScene(sceneName);
    //     enabled = false;
    // }
}
