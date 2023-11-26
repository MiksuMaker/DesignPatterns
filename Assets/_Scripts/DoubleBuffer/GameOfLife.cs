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

    CellState[,] currentGridState;
    CellState[,] bufferGridState;

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

        currentGridState = tileManager.RegisterGrid(width, length);
    }

    private void SetupGrids()
    {
        currentGridState = new CellState[width, length];
        bufferGridState = new CellState[width, length];
    }
    #endregion

    #region Lifecycle

    #endregion


}

public enum CellState
{
    dead, alive
}