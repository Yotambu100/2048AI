using System;

namespace OmegaProjectGame
{
    /// <summary>
    /// Class that hold the ulong that represent the board
    /// and the functions related to it
    /// </summary>
    public class BitBoard
    {
        //Board constant
        public const int BitColumnLength = 4;
        public const int BitRowLength = 4;
        public const int TileLength = 4;

        //randomizer
        private static Random randomizer = new Random();

        //main board
        private ulong mainBoard;
        
        //information about the board
        private ushort numberOfEmptyTiles;
        private uint score;
        private bool isWon;

        /// <summary>
        /// constructor
        /// </summary>
        public BitBoard()
        {
            this.isWon = false;
            this.score = 0;
            this.mainBoard = 0;
            this.numberOfEmptyTiles = Board.ColumnLength * Board.RowLength;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="board">board to be copied from</param>
        public BitBoard(BitBoard board)
        {
            this.isWon = board.isWon;
            this.mainBoard = board.mainBoard;
            this.numberOfEmptyTiles = board.numberOfEmptyTiles;
            this.score = board.score;
        }

        /// <summary>
        /// indexer
        /// used to get/set specific tiles in the board
        /// </summary>
        /// <param name="row">the row of the tile</param>
        /// <param name="column">the column of the tile</param>
        public int this[int row, int column]
        {
            get => (int) ((mainBoard & (0xfUL << TileLength * (row * BitColumnLength + column))) >>
                          TileLength * (row * BitColumnLength + column));
            set =>
                mainBoard = (mainBoard & ~(0xfUL << TileLength * (row * BitColumnLength + column))) |
                            ((ulong) value << TileLength * (row * BitColumnLength + column));
        }

        /// <summary>
        /// function that find random empty tile
        /// </summary>
        /// <returns>return TileCoordinates-that hold the row and column of the chosen tile</returns>
        public TileCoordinates GetRandomEmptyTile()
        {
            TileCoordinates[] emptyTiles = new TileCoordinates[this.numberOfEmptyTiles];
            int tilesCounter = 0;
            for (int row = 0; row < Board.RowLength && tilesCounter < this.numberOfEmptyTiles; row++)
            {
                for (int col = 0; col < Board.ColumnLength && tilesCounter < this.numberOfEmptyTiles; col++)
                {
                    if (this[row, col] == 0)
                    {
                        emptyTiles[tilesCounter] = new TileCoordinates(row, col);
                        tilesCounter++;
                    }
                }
            }

            return emptyTiles[randomizer.Next(0, this.numberOfEmptyTiles)];
        }

        /// <summary>
        /// equal
        /// </summary>
        /// <param name="obj">object to be compered from</param>
        /// <returns>true if the obj and current are same</returns>
        public override bool Equals(Object obj)
        {
            return this.mainBoard == ((BitBoard) obj).mainBoard;
        }

        /// <summary>
        /// The function try to shift the board down 
        /// </summary>
        /// <returns>true if the at least one tile moved/merged(meaning its a valid shift)</returns>
        public bool ShiftBoardDown()
        {
            ushort originalBoardCol;
            ushort shiftedBoardCol;
            bool didShift = false;

            // for each column in the board
            for (int col = 0; col < BitRowLength; col++)
            {
                // get the original column reading from down to up
                originalBoardCol = this.GetColFromDownUp(col);

                // use the original column to get the new column after shifting down 
                shiftedBoardCol = Controller.row_left_table[originalBoardCol].NewRow;

                // update the number of empty tiles in the board according to the difference between the old column to the new column
                this.numberOfEmptyTiles -= Controller.row_left_table[originalBoardCol].NumberOfEmptyTile;
                this.numberOfEmptyTiles +=
                    Controller.row_left_table[shiftedBoardCol ^ originalBoardCol].NumberOfEmptyTile;

                // update the score of the board according to the difference between the old column to the new column
                this.score -= Controller.row_left_table[originalBoardCol].Score;
                this.score += Controller.row_left_table[shiftedBoardCol ^ originalBoardCol].Score;

                // update if the board won the game after shifting the column
                this.isWon |= Controller.row_left_table[originalBoardCol].IsWon;

                // if the new column is not zero there is change between the old column to the new
                // meaning changing the column in the board to the new one and change didShift to true 
                //(the new column is being held as shiftedBoardCol=newCol^oldCol, if they are the same it will be zero )
                if (shiftedBoardCol != 0)
                {
                    didShift = true;
                    this.mainBoard =
                        (((mainBoard ^ (((ulong) (shiftedBoardCol >> 12) & 0xfUL) << (col * TileLength)))
                          ^ (((ulong) (shiftedBoardCol >> 8) & 0xfUl) << (col + 4) * TileLength))
                         ^ (((ulong) (shiftedBoardCol >> 4) & 0xfUl) << (col + 8) * TileLength))
                        ^ (((ulong) (shiftedBoardCol >> 0) & 0xfUl) << (col + 12) * TileLength);
                }
            }

            return didShift;
        }

        /// <summary>
        /// function that read a column from the board starting from the bottom till the top
        /// </summary>
        /// <param name="colNumber">the column number to be read from in the board</param>
        /// <returns>return column from the board starting from the bottom till the top</returns>
        public ushort GetColFromDownUp(int colNumber)
        {
            return (ushort) ((this.mainBoard >> (colNumber + 12) * TileLength) & 0xfL
                             | (this.mainBoard >> (colNumber + 7) * TileLength) & 0xf0UL
                             | (this.mainBoard >> (colNumber + 2) * TileLength) & 0xf00UL
                             | (this.mainBoard << (3 - colNumber) * TileLength) & 0xf000UL);
        }

        
        
        /// <summary>
        /// The function try to shift the board up 
        /// </summary>
        /// <returns>true if the at least one tile moved/merged(meaning its a valid shift)</returns>
        public bool ShiftBoardUp()
        {
            ushort originalBoardCol;
            ushort shiftedBoardCol;
            bool didShift = false;
            
            // for each column in the board
            for (int col = 0; col < BitRowLength; col++)
            {
                
                // get the original column reading from up to down
                originalBoardCol = this.GetColFromUpDown(col);

                // use the original column to get the new column after shifting up 
                shiftedBoardCol = Controller.row_left_table[originalBoardCol].NewRow;
                
                // update the number of empty tiles in the board according to the difference between the old column to the new column
                this.numberOfEmptyTiles -= Controller.row_left_table[originalBoardCol].NumberOfEmptyTile;
                this.numberOfEmptyTiles +=
                    Controller.row_left_table[shiftedBoardCol ^ originalBoardCol].NumberOfEmptyTile;

                // update the score of the board according to the difference between the old column to the new column
                this.score -= Controller.row_left_table[originalBoardCol].Score;
                this.score += Controller.row_left_table[shiftedBoardCol ^ originalBoardCol].Score;
                
                
                // update if the board won the game after shifting the column
                this.isWon |= Controller.row_left_table[originalBoardCol].IsWon;
                
                // if the new column is not zero there is change between the old column to the new
                // meaning changing the column in the board to the new one and change didShift to true 
                //(the new column is being held as shiftedBoardCol=newCol^oldCol, if they are the same it will be zero )
                if (shiftedBoardCol != 0)
                {
                    didShift = true;
                    this.mainBoard =
                        (((mainBoard ^ (((ulong) (shiftedBoardCol >> 0) & 0xfUL) << (col * TileLength)))
                          ^ (((ulong) (shiftedBoardCol >> 4) & 0xfUl) << (col + 4) * TileLength))
                         ^ (((ulong) (shiftedBoardCol >> 8) & 0xfUl) << (col + 8) * TileLength))
                        ^ (((ulong) (shiftedBoardCol >> 12) & 0xfUl) << (col + 12) * TileLength);
                }
            }

            return didShift;
        }


        /// <summary>
        /// function that read a column from the board starting from the top till the bottom
        /// </summary>
        /// <param name="colNumber">the column number to be read from in the board</param>
        /// <returns>return column from the board starting from the top till the bottom</returns>
        public ushort GetColFromUpDown(int colNumber)
        {
            return (ushort) ((this.mainBoard >> colNumber * TileLength) & 0xfUL
                             | (this.mainBoard >> (colNumber + 3) * TileLength) & 0xf0UL
                             | (this.mainBoard >> (colNumber + 6) * TileLength) & 0xf00UL
                             | (this.mainBoard >> (colNumber + 9) * TileLength) & 0xf000UL);
        }

        
        
        /// <summary>
        /// The function try to shift the board right 
        /// </summary>
        /// <returns>true if the at least one tile moved/merged(meaning its a valid shift)</returns>
        public bool ShiftBoardRight()
        {
            ushort originalBoardRow;
            ushort shiftedBoardRow;
            bool didShift = false;
            
            // for each row in the board
            for (int row = 0; row < BitColumnLength; row++)
            {
                // get the original row reading from right to left
                originalBoardRow = this.GetRowFromRightLeft(row);

                // use the original column to get the new column after shifting right 
                shiftedBoardRow = Controller.row_left_table[originalBoardRow].NewRow;
                
                // update the number of empty tiles in the board according to the difference between the old column to the new column
                this.numberOfEmptyTiles -= Controller.row_left_table[originalBoardRow].NumberOfEmptyTile;
                this.numberOfEmptyTiles +=
                    Controller.row_left_table[shiftedBoardRow ^ originalBoardRow].NumberOfEmptyTile;

                // update the score of the board according to the difference between the old column to the new column
                this.score -= Controller.row_left_table[originalBoardRow].Score;
                this.score += Controller.row_left_table[shiftedBoardRow ^ originalBoardRow].Score;

                // update if the board won the game after shifting the column
                this.isWon |= Controller.row_left_table[originalBoardRow].IsWon;
                
                // if the new row is not zero there is change between the old row to the new
                // meaning changing the row in the board to the new one and change didShift to true 
                //(the new row is being held as shiftedBoardRow=newRow^oldRow, if they are the same it will be zero )
                if (shiftedBoardRow != 0)
                {
                    didShift = true;
                    this.mainBoard = (((mainBoard ^
                                        (((ulong) (shiftedBoardRow >> 12) & 0xfUL) <<
                                         row * BitRowLength * TileLength))
                                       ^ (((ulong) (shiftedBoardRow >> 8) & 0xfUl) <<
                                          ((1 + row * BitRowLength) * TileLength)))
                                      ^ (((ulong) (shiftedBoardRow >> 4) & 0xfUl) <<
                                         ((2 + row * BitRowLength) * TileLength))
                                      ^ (((ulong) (shiftedBoardRow >> 0) & 0xfUl) <<
                                         ((3 + row * BitRowLength) * TileLength)));
                }
            }

            return didShift;
        }

        /// <summary>
        /// function that read a row from the board starting from the right till the left
        /// </summary>
        /// <param name="rowNumber">the row number to be read from in the board</param>
        /// <returns></returns>
        public ushort GetRowFromRightLeft(int rowNumber)
        {
            return (ushort) ((this.mainBoard >> ((3 + (rowNumber * BitRowLength)) * TileLength) & 0xfL
                              | (this.mainBoard >> ((1 + (rowNumber * BitRowLength)) * TileLength) & 0xf0UL)
                              | ((this.mainBoard >> (1 + (rowNumber * BitRowLength)) * TileLength) & 0xfUL)
                              << (2 * TileLength)
                              | (this.mainBoard >> (rowNumber * BitRowLength * TileLength) & 0xfUL)
                              << (3 * TileLength)));
        }

        
        
        
        /// <summary>
        /// The function try to shift the board left 
        /// </summary>
        /// <returns>true if the at least one tile moved/merged(meaning its a valid shift)</returns>
        public bool ShiftBoardLeft()
        {
            ushort originalBoardRow;
            ushort shiftedBoarRow;
            bool didShift = false;
            
            // for each row in the board
            for (int row = 0; row < BitColumnLength; row++)
            {
                // get the original row reading from left to right
                originalBoardRow = (ushort) ((this.mainBoard >> BitRowLength * TileLength * row) & 0xffffUL);
                
                // use the original column to get the new column after shifting left
                shiftedBoarRow = Controller.row_left_table[originalBoardRow]
                    .NewRow;
                
                // update the number of empty tiles in the board according to the difference between the old column to the new column
                this.numberOfEmptyTiles -= Controller.row_left_table[originalBoardRow].NumberOfEmptyTile;
                this.numberOfEmptyTiles +=
                    Controller.row_left_table[shiftedBoarRow ^ originalBoardRow].NumberOfEmptyTile;

                // update the score of the board according to the difference between the old column to the new column
                this.score -= Controller.row_left_table[originalBoardRow].Score;
                this.score += Controller.row_left_table[shiftedBoarRow ^ originalBoardRow].Score;

                // update if the board won the game after shifting the column
                this.isWon |= Controller.row_left_table[originalBoardRow].IsWon;

                // if the new row is not zero there is change between the old row to the new
                // meaning changing the row in the board to the new one and change didShift to true 
                //(the new row is being held as shiftedBoardRow=newRow^oldRow, if they are the same it will be zero )
                if (shiftedBoarRow != 0)
                {
                    didShift = true;
                    this.mainBoard = mainBoard ^ ((ulong) shiftedBoarRow << row * BitRowLength * TileLength);
                }
            }

            return didShift;
        }


        /// <summary>
        /// function that return a uniq key for the current board state
        /// </summary>
        /// <returns>return a uniq key for the current board state</returns>
        public ulong GetBitBoardKey()
        {
            return this.mainBoard;
        }

        public ushort NumberOfEmptyTiles
        {
            get => numberOfEmptyTiles;
            set => numberOfEmptyTiles = value;
        }

        public uint Score
        {
            get => score;
            set => score = value;
        }

        public bool IsWon
        {
            get => isWon;
            set => isWon = value;
        }
    }
}