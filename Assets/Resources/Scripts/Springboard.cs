using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Springboard : MonoBehaviour
{
    private const float TEMP_DISABLE_DURATION = 0.25f;

    private new Collider2D collider2D;

    void Awake() => collider2D = GetComponent<Collider2D>();

    private IEnumerator Co_TempDisable()
    {
        collider2D.enabled = false;
        yield return new WaitForSeconds(TEMP_DISABLE_DURATION);
        collider2D.enabled = true;
    }

    public void TempDisable() => StartCoroutine(Co_TempDisable());
}
