using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class TilemapAnalysis : MonoBehaviour
{
    private CompositeCollider2D tilemapCollider;

    void Awake() => tilemapCollider = GetComponent<CompositeCollider2D>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Debug.Log($"Path Count: {tilemapCollider.pathCount}");
}
