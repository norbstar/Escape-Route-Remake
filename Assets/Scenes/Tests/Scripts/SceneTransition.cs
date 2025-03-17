using UnityEngine;

using Tests;

using Zone;

public class SceneTransition : MonoBehaviour
{
    private OrthographicSizeFX orthographicSizeFX;
    private ZoneManager zoneManager;

    void Awake() => orthographicSizeFX = FindFirstObjectByType<OrthographicSizeFX>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoneManager = FindAnyObjectByType<ZoneManager>();
        orthographicSizeFX.Invoke();
    }

    void OnEnable() => orthographicSizeFX.Subscribe(OnComplete);

    void OnDisable() => orthographicSizeFX.Unsubscribe();

    private void OnComplete(OrthographicSizeFX instance)
    {
        var basePlayer = FindAnyObjectByType<BasePlayer>();

        if (basePlayer == null)
        {
            zoneManager.ResolvePlayer();
        }
    }
}
