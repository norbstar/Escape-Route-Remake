using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class MergeTiilemapTest : MonoBehaviour
{
    [SerializeField] Tilemap originalTilemap;
    [SerializeField] List<Tilemap> tilemaps;

    public int width;
    public int height;

    public Tile[,] tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var tilemap in tilemaps)
        {
            Analyse(tilemap);
        }
    }

    private void Analyse(Tilemap tilemap)
    {
        Debug.Log("Analyse tilemap: " + tilemap.name);

        BoundsInt bounds = tilemap.cellBounds;

        Debug.Log("Bounds Size: " + bounds.size);

        // TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // for (int x = 0; x < bounds.size.x; x++) {
        //     for (int y = 0; y < bounds.size.y; y++) {
        //         var tile = allTiles[x + y * bounds.size.x];
                
        //         if (tile != null)
        //         {
        //             Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    
        //             originalTilemap.SetTile(new Vector3Int(x, y, 0), tile);
        //         }
        //     }
        // }

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(position.x, position.y, position.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            
            if (tilemap.HasTile(localPlace))
            {
                Debug.Log($"X: {position.x} Y: {position.y} Z: {position.z} Place: {place} Name: {tilemap.GetTile(localPlace).name}");
            }
        }
    }

#if false
    public void ImportTiles() {
        ClonedTile clonedTile;

        int w_y = 0; // X coordinate in worldGraph

        for(int y = -height/2; y<height/2; y++ ) {
            int w_x = 0; // Y coordinate in worldGraph

            for (int x = -width/2; x<width/2; x++ ) {
                Vector3Int pos = new Vector3Int( x, y, 0 );

                TileBase tile = floor.GetTile(pos);

                if( tile != null ) {
                    clonedTile = new ClonedTile( w_x, w_y, TileType.Floor, false );
                    tiles[w_x, w_y] = clonedTile;
                    originalTiles.Add( clonedTile, tile );
                }

                tile = walls.GetTile( pos );

                if (tile != null ) {
                    clonedTile = new ClonedTile( w_x, w_y, TileType.Wall, false );
                    tiles[w_x, w_y] = clonedTile;
                    originalTiles.Add( clonedTile, tile );
                }

                tile = door.GetTile( pos );
                if(tile!= null ) {
                    clonedTile = new ClonedTile( w_x, w_y, TileType.Door, false );
                    tiles[w_x, w_y] = clonedTile;
                    originalTiles.Add( clonedTile, tile );
                }

                w_x++;
            }
            w_y++;
        }
    }
#endif
}
