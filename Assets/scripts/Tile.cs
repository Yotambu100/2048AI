namespace DefaultNamespace
{
    
    /// <summary>
    /// Class that used to represent tiles location and values
    /// </summary>
    public class Tile{
        private int rowIndex;
        private int colIndex;
        private int value;
        
        
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="row">the row of the tile to be represented</param>
        /// <param name="col">the column of the tile to be represented</param>
        /// <param name="value">the value of the tile to be represented</param>s
        public Tile(int row, int col,int value)
        {
            this.rowIndex = row;
            this.colIndex = col;
            this.value = value;
        }
        
        public Tile(){}
        public int RowIndex { get => rowIndex; set => rowIndex = value; }
        public int ColIndex { get => colIndex; set => colIndex = value; }
        public int Value { get => value; set => this.value = value; }
        
        
    }
   
    
}