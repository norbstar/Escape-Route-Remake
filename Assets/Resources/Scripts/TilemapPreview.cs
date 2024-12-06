using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapPreview : MonoBehaviour
{
    private Tilemap tileMap;

    void Awake() => tileMap = GetComponent<Tilemap>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var bounds = tileMap.cellBounds;
        Debug.Log($"Bounds: {bounds}");

        // for (int xItr = 0; xItr < bounds.size.x; xItr++)
        // {
        //     for (int yItr = 0; yItr < bounds.size.y; yItr++)
        //     {
        //         Debug.Log($"Tile: [{xItr}, {yItr}]");
        //     }
        // }

        var tiles = tileMap.GetTilesBlock(bounds);

        for (int index = 0; index < tiles.Length; index++)
        {
            var tile = (Tile) tiles[index];

            if (!tile.IsUnityNull())
            {
                Debug.Log($"Tile: {tile.sprite.name}");
            }
        }
    }
}
