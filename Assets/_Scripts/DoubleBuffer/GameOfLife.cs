using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    #region References
    TileManager tileManager;
    #endregion

    #region Variables
    [Header("Space")]
    [SerializeField] int width = 10;
    [SerializeField] int length = 10;

    CellState[,] currentGrid;
    CellState[,] bufferGrid;

    [Header("Ticking Speed")]
    [SerializeField, Range(0f, 5f)]
    float tickLength = 1f;
    float timeSinceTick = 0f;
    #endregion

    #region Setup
    private void Start()
    {
        tileManager = GetComponent<TileManager>();

        SetupGrids();
    }

    private void SetupGrids()
    {
        currentGrid = new CellState[width, length];
        bufferGrid = new CellState[width, length];

        currentGrid = tileManager.RegisterGrid(width, length);
    }
    #endregion

    #region Lifecycle
    private void Update()
    {
        HandleGame();
    }

    private void HandleGame()
    {
        // Increase time
        timeSinceTick += Time.deltaTime;

        if (timeSinceTick > tickLength)
        {
            // Update the buffer grid
            UpdateBufferGrid();


            // Tick the simulation

            // Draw the new grid state
            tileManager.DrawGrid(currentGrid, width, length);

            // Reset
            timeSinceTick = 0f;
        }
    }
    #endregion

    #region Rules
    private void UpdateBufferGrid()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                // Update according to RULES:

                // 1. Fewer than 2 neighbours           -> DIE
                // 2. 2 or 3 neighbours                 -> LIVE
                // 3. more than 3 neighbours            -> DIE
                // 4. dead cell with 3 live neighbours  -> NEW CELL BORN

                int neighbours = GetLiveNeighbours(x, y);

                if (neighbours < 2)
                {
                    bufferGrid[x, y] = CellState.dead;
                }
                else if (bufferGrid[x, y] == CellState.alive && neighbours < 4)
                {
                    bufferGrid[x, y] = CellState.alive;
                }
                else if (bufferGrid[x, y] == CellState.alive && neighbours > 3)
                {
                    bufferGrid[x, y] = CellState.dead;
                }
                else if (bufferGrid[x, y] == CellState.dead && neighbours == 3)
                {
                    bufferGrid[x, y] = CellState.alive;
                }

            }
        }

        // Update the current tiles
        //CellState[,] temp = new CellState[width, length];
        //CellState[,] temp = currentGrid;
        currentGrid = bufferGrid;
    }

    private int GetLiveNeighbours(int _X, int _Y)
    {
        int livingNeighbours = 0;

        for (int x = _X - 1; x <= _X + 1; x++)
        {
            for (int y = _Y - 1; y <= _Y + 1; y++)
            {
                if (!(x == _X & y == _Y) && x >= 0 && y >= 0 && x < width && y < length)
                {
                    // current i,j is not x,y
                    //if (currentGridState[i, j] == true)
                    if (currentGrid[x, y] == CellState.alive)
                    {
                        livingNeighbours++;
                    }
                }
            }
        }
        return livingNeighbours;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 leftBottom = transform.position;
    }
}

public enum CellState
{
    dead, alive
}