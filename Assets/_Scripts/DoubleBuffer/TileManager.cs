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
        //PaintTileAt(0, 0);
        //PaintTileAt(1, 0);
        //PaintTileAt(0, 2);
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

    #region Get tiles
    public CellState[,] RegisterGrid(int width, int length)
    {
        CellState[,] grid = new CellState[width,length];

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Check if there is a cell at the pos
                if (IsThereTileAt(x, y))
                {
                    // If so, register it to the Grid
                    grid[x, y] = CellState.alive;
                    Debug.DrawRay(new Vector3(x + 0.5f, 0f, y + 0.5f), Vector3.up, Color.green, 3f);
                }
                else
                {
                    grid[x, y] = CellState.dead;
                }

            }
        }

        return grid;
    }
    #endregion

}
