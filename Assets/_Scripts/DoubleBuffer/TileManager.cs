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
    Tilemap bufferTileMap;

    [Header("Cells")]
    [SerializeField] Tile lifeCell;
    [SerializeField] Tile newCell;
    [SerializeField] Tile deadCell;
    [SerializeField] Tile decayCell;
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

    public void PaintTileAt(int x, int y, Cell cell)
    {
        Vector3Int pos = new Vector3Int(x, y);

        switch(cell.state, cell.age)
        {
            case (Cell.State.DEAD, Cell.Age.BURIED):

                tileMap.SetTile(pos, deadCell);
                break;

            case (Cell.State.DEAD, Cell.Age.DECAYING):

                tileMap.SetTile(pos, decayCell);
                break;

            case (Cell.State.ALIVE, Cell.Age.MATURE):

                tileMap.SetTile(pos, lifeCell);
                break;

            case (Cell.State.ALIVE, Cell.Age.BORN):

                tileMap.SetTile(pos, newCell);
                break;

        }
        //tileMap.SetTile(pos, lifeCell);
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

    #region Higher Functions
    //public CellState[,] RegisterGrid(int width, int length)
    //{
    //    CellState[,] grid = new CellState[width,length];

    //    for (int y = 0; y < length; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            // Check if there is a cell at the pos
    //            if (IsThereTileAt(x, y))
    //            {
    //                // If so, register it to the Grid
    //                grid[x, y] = CellState.ALIVE;
    //                Debug.DrawRay(new Vector3(x + 0.5f, 0f, y + 0.5f), Vector3.up, Color.green, 1f);
    //            }
    //            else
    //            {
    //                grid[x, y] = CellState.DEAD;
    //                //Debug.DrawRay(new Vector3(x + 0.5f, 0f, y + 0.5f), Vector3.up, Color.magenta, 1f);
    //            }

    //        }
    //    }

    //    return grid;
    //}

    public Cell[,] RegisterGrid(int width, int length)
    {

        Cell[,] grid = new Cell[width, length];

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell newCell = new Cell();

                // Check if there is a cell at the pos
                if (IsThereTileAt(x, y))
                {
                    // If so, register it to the Grid
                    newCell.NewCell(false);
                    //Debug.DrawRay(new Vector3(x + 0.5f, 0f, y + 0.5f), Vector3.up, Color.green, 1f);
                }
                else
                {
                    newCell.DeadCell(false);
                    //Debug.DrawRay(new Vector3(x + 0.5f, 0f, y + 0.5f), Vector3.up, Color.magenta, 1f);
                }

                grid[x, y] = newCell;
            }
        }

        return grid;
    }

    //public void DrawGrid(CellState[,] grid, int width, int length)
    //{
    //    for (int y = 0; y < length; y++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            // Check if there is a cell at the pos
    //            if (grid[x,y] == CellState.ALIVE)
    //            {
    //                // If so, draw it
    //                PaintTileAt(x, y);
    //            }
    //            else
    //            {
    //                ClearTileAt(x, y);
    //            }

    //        }
    //    }
    //}

    public void DrawGrid(Cell[,] grid, int width, int length)
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Check if there is a cell at the pos
                //if (grid[x, y].state == Cell.State.ALIVE)
                //{
                //    // If so, draw it
                //    PaintTileAt(x, y);
                //}
                //else
                //{
                //    ClearTileAt(x, y);
                //}

                PaintTileAt(x, y, grid[x, y]);
            }
        }
    }
    #endregion

}
