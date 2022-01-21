
namespace OmegaProjectGame
{
    /// <summary>
    /// Class that used to represent tiles location in the board
    /// </summary>
    public class TileCoordinates
    {
        private int row;
        private int col;
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="row">the row of the tile to be represented</param>
        /// <param name="col">the column of the tile to be represented</param>
        public TileCoordinates(int row,int col)
        {
            this.row = row;
            this.col = col;
        }
        
        public int Col { get => col; set => col = value; }
        public int Row { get => row; set => row = value; }
    }
}