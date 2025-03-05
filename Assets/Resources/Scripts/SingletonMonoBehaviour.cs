using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour
{
    public static SingletonMonoBehaviour<T> Instance;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}