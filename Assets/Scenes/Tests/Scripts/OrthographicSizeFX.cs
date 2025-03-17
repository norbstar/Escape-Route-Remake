using System.Collections;

using UnityEngine;

using Cinemachine;

public class OrthographicSizeFX : MonoBehaviour
{
    [SerializeField] float startSize = 0f;
    [SerializeField] float endSize = 5f;
    [SerializeField] float transitionSpeed = 1f;
    [SerializeField] float preDelay = 1f;

    [Header("Stats")]
    [SerializeField] [ReadOnly] float value;

    public delegate void OnComplete(OrthographicSizeFX instance);

    private CinemachineVirtualCamera virtualCamera;
    private OnComplete onComplete;

    // private void Awake() => virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();

    // Start is called before the first frame update
    // IEnumerator Start()
    // {
    //     virtualCamera.m_Lens.OrthographicSize = startScale;

    //     if (startScale != endScale)
    //     {
    //         yield return new WaitForSeconds(preDelay);
    //         StartCoroutine(Co_Scale());
    //     }
    // }

    public void Subscribe(OnComplete onComplete) => this.onComplete = onComplete;

    public void Unsubscribe() => onComplete = null;

    public void Invoke()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        }

        virtualCamera.m_Lens.OrthographicSize = startSize;
        StartCoroutine(Co_Init());
    }

    private IEnumerator Co_Init()
    {
        float elsapsedTime = 0f;
        yield return new WaitForSeconds(preDelay);
        
        while (virtualCamera.m_Lens.OrthographicSize != endSize)
        {
            elsapsedTime += Time.deltaTime;
            var scale = Mathf.Lerp(startSize, endSize, elsapsedTime * transitionSpeed);
            
            if (startSize < endSize)
            {
                scale = Mathf.Clamp(scale, startSize, endSize);
            }
            else
            {
                scale = Mathf.Clamp(scale, endSize, startSize);
            }

            virtualCamera.m_Lens.OrthographicSize = scale;
            value = scale;
            yield return null;
        }

        onComplete?.Invoke(this);
    }
}
