using System.Collections;

using UnityEngine;

using UI;

public class AnalyticsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Transform viewUI;

    [Header("UI Attributes")]
    [SerializeField] AttributeUI updatesUI;
    [SerializeField] AttributeUI fixedUpdatesUI;

    private int updatesPerSecond;
    private int fixedUpdatesPerSecond;
    private int updateCount;
    private int fixedUpdateCount;

    private IEnumerator Co_MonitorCounts()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(1f);
            updatesPerSecond = updateCount;
            fixedUpdatesPerSecond = fixedUpdateCount;
            updateCount = fixedUpdateCount = 0;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => StartCoroutine(Co_MonitorCounts());
    
    private void UpdateUI()
    {
        if (updatesUI != null)
        {
            updatesUI.Value = updatesPerSecond.ToString();
        }

        if (fixedUpdatesUI != null)
        {
            fixedUpdatesUI.Value = fixedUpdatesPerSecond.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ++updateCount;
        UpdateUI();
    }

    void FixedUpdate() => ++fixedUpdateCount;

    public int UpdatesPerSecond => updatesPerSecond;
    
    public int FixedUpdatesPerSecond => fixedUpdatesPerSecond;

    public void SetActive(bool active) => viewUI.gameObject.SetActive(active);
    
    public void FlipView() => SetActive(!viewUI.gameObject.activeSelf);
}