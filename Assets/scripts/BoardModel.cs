using DefaultNamespace;
using OmegaProjectGame;


/// <summary>
/// class that represent the board in the UI
/// </summary>
public class BoardModel
{
    private const int EmptyTile = 0;


    private Tile[,] tiles = new Tile[Board.RowLength, Board.ColumnLength];


    /// <summary>
    /// constructor that initializing the board with empty tiles
    /// </summary>
    public BoardModel()
    {
        for (int row = 0; row < Board.RowLength; row++)
        {
            for (int col = 0; col < Board.ColumnLength; col++)
            {
                Tile tile = new Tile(row, col, EmptyTile);
                tiles[row, col] = tile;
            }
        }
    }

    /// <summary>
    /// function that load the given tiles in the board
    /// </summary>
    /// <param name="newTiles">the new tiles to be updated</param>
    public void initBoard(params Tile[] newTiles)
    {
        foreach (Tile newTile in newTiles)
        {
            this.tiles[newTile.RowIndex, newTile.ColIndex].Value = newTile.Value;
        }
    }

    
    public Tile this[int row, int col] => this.tiles[row, col];
}