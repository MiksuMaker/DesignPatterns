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

    //CellState[,] currentGrid;
    //CellState[,] bufferGrid;

    Cell[,] currentGrid;
    Cell[,] bufferGrid;

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
        //currentGrid = new CellState[width, length];
        //bufferGrid = new CellState[width, length];

        currentGrid = new Cell[width, length];
        bufferGrid = new Cell[width, length];

        currentGrid = tileManager.RegisterGrid(width, length);
        bufferGrid = tileManager.RegisterGrid(width, length);
    }
    #endregion

    #region Lifecycle
    private void Update()
    {
        HandleGame();
    }

    private void HandleGame()
    {
        // COROUTINE MODE - For slower inspection
        //if (coroutineRunningDone)
        //{
        //    StartCoroutine(BufferCoroutine());
        //}

        // Increase time
        timeSinceTick += Time.deltaTime;

        if (timeSinceTick > tickLength)
        {
            // Update the buffer grid
            UpdateBufferGrid();

            // Update the current tiles
            LoadBufferIntoCurrentGrid();

            // Draw the new grid state
            tileManager.DrawGrid(currentGrid, width, length);

            // Reset
            timeSinceTick = 0f;
        }
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
        LoadBufferIntoCurrentGrid();

        // Draw
        tileManager.DrawGrid(currentGrid, width, length);

        coroutineRunningDone = true;
    }

    private void LoadBufferIntoCurrentGrid()
    {
        //Debug.Log("=== LOADING BUFFER to CURRENT grid ===");

        // V1   ->    CHANGES REFERENCE TO BUFFERGRID
        //currentGrid = bufferGrid; // CHANGES THE REFERENCE TO THE BUFFERGRID


        // V2   ->    WORKS
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                currentGrid[x, y] = bufferGrid[x, y];
            }
        }


        // V3   ->    DOESN'T DO ANYTHING
        //CellState[,] temp = currentGrid;
        //currentGrid = bufferGrid;
        //currentGrid = temp;
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
        Vector3 dir = Vector3.up * 0f;


        int neighbours = GetLiveNeighbours(x, y);

        if (currentGrid[x, y].state == Cell.State.DEAD)
        {

            if (neighbours == 3) // REBIRTH
            {
                //bufferGrid[x, y].state = Cell.State.ALIVE;
                //bufferGrid[x, y].age = Cell.Age.BORN;

                bufferGrid[x, y].NewCell();
            }
            else
            {
                bufferGrid[x, y].DeadCell(false);
            }
        }
        else
        {
            if (neighbours < 2) // TOO LONELY
            {
                //bufferGrid[x, y].state = Cell.State.DEAD;
                bufferGrid[x, y].DeadCell(true);
            }
            else if (neighbours < 4) // LIVABLE
            {
                //bufferGrid[x, y].state = Cell.State.ALIVE;
                bufferGrid[x, y].NewCell(false);
            }
            else if (neighbours > 3) // TOO CROWDED
            {
                //bufferGrid[x, y].state = Cell.State.DEAD;
                bufferGrid[x, y].DeadCell(true);
            }
            //else if (currentGrid[x, y].state == Cell.State.ALIVE)
            //{

            //}

        }


    }

    private int GetLiveNeighbours(int _X, int _Y)
    {
        int livingNeighbours = 0;

        Vector3 start = transform.position + offset + new Vector3(_X, 0f, _Y);
        Vector3 pos;

        for (int x = _X - 1; x <= _X + 1; x++)
        {
            for (int y = _Y - 1; y <= _Y + 1; y++)
            {


                if (!(x == _X & y == _Y) && x >= 0 && y >= 0 && x < width && y < length)
                {
                    pos = new Vector3(x, 0f, y) + offset;


                    if (currentGrid[x, y].state == Cell.State.ALIVE)
                    {
                        livingNeighbours++;

                        //Debug.DrawRay(pos, Vector3.up, Color.green, coroutineWait);
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
    DEAD, ALIVE,
}

public struct Cell
{
    public enum State
    {
        DEAD, ALIVE
    }

    public enum Age
    {
        BORN, MATURE, DECAYING, BURIED
    }

    //public State state = State.DEAD;
    //public Age age = Age.BURIED;
    public State state;
    public Age age;

    public void NewCell(bool brandNew = true)
    {
        state = State.ALIVE;

        if (brandNew) { age = Age.BORN; }
        else { age = Age.MATURE; }
    }

    public void DeadCell(bool freshDeath = false)
    {
        state = State.DEAD;

        if (freshDeath) { age = Age.DECAYING; }
        else { age = Age.BURIED; }
    }
}