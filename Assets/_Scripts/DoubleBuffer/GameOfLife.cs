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

    Vector3 offset = new Vector3(0.5f, 0f, 0.5f);

    CellState[,] currentGrid;
    CellState[,] bufferGrid;

    [Header("Ticking Speed")]
    [SerializeField, Range(0f, 5f)]
    float tickLength = 1f;
    float timeSinceTick = 0f;

    IEnumerator bufferUpdaterCoroutine;
    bool coroutineRunningDone = true;
    [SerializeField] float coroutineWait = 0.5f;
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

        if (coroutineRunningDone)
        {
            StartCoroutine(BufferCoroutine());
        }

        //// Increase time
        //timeSinceTick += Time.deltaTime;

        //if (timeSinceTick > tickLength)
        //{
        //    // Update the buffer grid
        //    UpdateBufferGrid();

        //    // Update the current tiles
        //    currentGrid = bufferGrid;

        //    // Draw the new grid state
        //    tileManager.DrawGrid(currentGrid, width, length);

        //    // Reset
        //    timeSinceTick = 0f;
        //}
    }

    IEnumerator BufferCoroutine()
    {
        coroutineRunningDone = false;

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                UpdateBufferGridCell(x, y);

                yield return new WaitForSeconds(coroutineWait);
            }
        }

        // Switch
        currentGrid = bufferGrid;

        // Draw
        tileManager.DrawGrid(currentGrid, width, length);

        coroutineRunningDone = true;
    }
    #endregion

    #region Rules
    private void UpdateBufferGrid()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                UpdateBufferGridCell(x, y);
            }
        }
    }

    private void UpdateBufferGridCell(int x, int y)
    {
        // Update according to RULES:

        // 1. Fewer than 2 neighbours           -> DIE
        // 2. 2 or 3 neighbours                 -> LIVE
        // 3. more than 3 neighbours            -> DIE
        // 4. dead cell with 3 live neighbours  -> NEW CELL BORN

        Vector3 start = transform.position + offset + new Vector3(x, 0f, y);
        Vector3 dir = Vector3.up;

        int neighbours = GetLiveNeighbours(x, y);

        //if (currentGrid[x, y] == CellState.ALIVE && neighbours < 2) // TOO LONELY
        if (neighbours < 2) // TOO LONELY
        {
            bufferGrid[x, y] = CellState.DEAD;
            Debug.DrawRay(start, dir, Color.blue, tickLength);

        }
        else if (currentGrid[x, y] == CellState.ALIVE && neighbours < 4) // LIVABLE
        {
            bufferGrid[x, y] = CellState.ALIVE;
            Debug.DrawRay(start, dir, Color.green, tickLength);

        }
        else if (currentGrid[x, y] == CellState.ALIVE && neighbours > 3) // TOO CROWDED
        {
            bufferGrid[x, y] = CellState.DEAD;
            Debug.DrawRay(start, dir, Color.red, tickLength);

        }
        else if (currentGrid[x, y] == CellState.DEAD && neighbours == 3) // REBIRTH
        {
            bufferGrid[x, y] = CellState.ALIVE;
            Debug.DrawRay(start, dir, Color.yellow, tickLength);
        }
        else if (currentGrid[x, y] == CellState.ALIVE)
        {
            Debug.DrawRay(start, dir * 2f, Color.magenta, tickLength);
        }
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
                    if (currentGrid[x, y] == CellState.ALIVE)
                    {
                        livingNeighbours++;
                    }
                }
            }
        }

        //Vector3 start = transform.position + offset + new Vector3(_X, 0f, _Y);
        //Vector3 dir = Vector3.up;

        //if (livingNeighbours < 2)       { Debug.DrawRay(start, dir, Color.blue, tickLength); }
        //else if (livingNeighbours < 4)  { Debug.DrawRay(start, dir, Color.green, tickLength); }
        //else if (livingNeighbours > 3)  { Debug.DrawRay(start, dir, Color.red, tickLength); }
        //else if (livingNeighbours == 3) { Debug.DrawRay(start, dir, Color.yellow, tickLength); }

        return livingNeighbours;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 leftBottom = transform.position;
        Vector3 rightBottom = leftBottom + Vector3.right * width;
        Vector3 leftTop = leftBottom + Vector3.forward * length;
        Vector3 rightTop = leftTop + rightBottom;

        Gizmos.color = Color.red;

        // Draw lines
        Gizmos.DrawLine(leftBottom, rightBottom);
        Gizmos.DrawLine(leftBottom, leftTop);
        Gizmos.DrawLine(rightTop, rightBottom);
        Gizmos.DrawLine(rightTop, leftTop);
    }
}

public enum CellState
{
    DEAD, ALIVE
}