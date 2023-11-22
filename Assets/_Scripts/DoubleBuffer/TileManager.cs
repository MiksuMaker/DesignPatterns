using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Tilemap tileMap;
    [SerializeField]
    Tile lifeCell;
    #endregion

    #region Setup
    private void Start()
    {

    }
    #endregion

    #region Tile Handling
    public void PaintTileAt(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);
        tileMap.SetTile(pos, lifeCell);
    }

    public void ClearTileAt(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);
        tileMap.SetTile(pos, null);
    }

    public bool IsThereTileAt(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);
        Tile temp = tileMap.GetTile(pos) as Tile;

        if (temp != null) { return true; } else { return false; }
    }

    public Tile GetTileAt(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);
        Tile temp = tileMap.GetTile(pos) as Tile;

        return temp;
    }
    #endregion

}
