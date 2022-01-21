using System;
using DefaultNamespace;
using OmegaProjectGame;
using UnityEngine;


/// <summary>
/// the class that responsible of adding/removing/changing every tile in the board
/// </summary>
public class TileMovement : MonoBehaviour
{
    // BoardModel board;
    public GameObject Tile2;
    public GameObject Tile4;
    public GameObject Tile8;
    public GameObject Tile16;
    public GameObject Tile32;
    public GameObject Tile64;
    public GameObject Tile128;
    public GameObject Tile256;
    public GameObject Tile512;
    public GameObject Tile1024;
    public GameObject Tile2048;
    public GameObject Tile4096;
    public GameObject Tile8192;
    public GameObject Tile16384;
    public GameObject Tile32768;



    /// <summary>
    /// function that gives the corresponding GameObject to the value given
    /// </summary>
    /// <param name="value">the number of the tile that the GameObject will be</param>
    /// <returns>return the responding GameObject to the value given</returns>
    private GameObject convertNumberToCube(int value)
    {
        switch (value)
        {
            case 1: return Tile2;
            case 2: return Tile4;
            case 3: return Tile8;
            case 4: return Tile16;
            case 5: return Tile32;
            case 6: return Tile64;
            case 7: return Tile128;
            case 8: return Tile256;
            case 9: return Tile512;
            case 10: return Tile1024;
            case 11: return Tile2048;
            case 12: return Tile4096;
            case 13: return Tile8192;
            case 14: return Tile16384;
            case 15: return Tile32768;
        }
        
        // will not get to here
        throw new InvalidOperationException("This number is not supported. yet");
    }


    /// <summary>
    /// function that one tile to the board in the location given
    /// </summary>
    /// <param name="cubeToInitFrom">the GameObject of the tile</param>
    /// <param name="row">the row the tile will be in</param>
    /// <param name="col">the column  the tile will be in</param>
    private void initTile(GameObject cubeToInitFrom, int row, int col)
    {
        GameObject tile = Instantiate(cubeToInitFrom);
        simpleTileMovement simpleTileMovementScript = tile.GetComponent<simpleTileMovement>();
        simpleTileMovementScript.Row = row;
        simpleTileMovementScript.Column = col;
    }

    /// <summary>
    /// function that gets a board and add every tile to the UI
    /// </summary>
    /// <param name="board">the board that the function will add to the UI</param>
    public void UpdateBoard(BoardModel board)
    {
        foreach (Transform transformChld in transform)
        {
            Destroy(transformChld.gameObject);
        }

        Tile tile;
        for (int row = 0; row < Board.RowLength; row++)
        {
            for (int col = 0; col < Board.ColumnLength; col++)
            {
                tile = board[row, col];
                if (tile.Value != 0)
                {
                    initTile(convertNumberToCube(tile.Value), tile.RowIndex, tile.ColIndex);
                }
            }
        }
    }
}