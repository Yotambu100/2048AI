using System;

namespace OmegaProjectGame
{
    public class Board
    {
        // ColumnLength and RowLength must be equal
        public const int ColumnLength = 4;

        public const int RowLength = 4;
        // private int numberOfEmptyTiles;

        private BitBoard board;


        /// <summary>
        /// empty constructor
        /// </summary>
        public Board()
        {
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="board">the board to be copied from</param>
        private Board(Board board)
        {
            this.board = new BitBoard(board.board);
        }

        /// <summary>
        /// function that return a new duplicate board from the given board
        /// </summary>
        /// <returns>return new duplicate board</returns>
        public Board CreateBackUpBoard()
        {
            return new Board(this);
        }

        /// <summary>
        /// function that return the board as long
        /// </summary>
        /// <returns>return the board as long</returns>
        public ulong GetBoardKey()
        {
            return this.board.GetBitBoardKey();
        }

        /// <summary>
        /// indexer for the board that will be used to reach specifics tile
        /// </summary>
        /// <param name="row">the row of the tile</param>
        /// <param name="col">the column of the tile</param>
        public int this[int row, int col]
        {
            get => board[row, col];
            set => board[row, col] = value;
        }

        /// <summary>
        /// function that initializing the board at the start of the game
        /// </summary>
        public void InitializeBoard()
        {
            //create the bitboard
            this.board = new BitBoard();

            //place 2 random tiles in 2 random positions
            for (int i = 0; i < 2; i++)
            {
                //add the tiles
                AddNewTile();
            }
        }

        public override bool Equals(Object obj)
        {
            Board board = (Board) obj;
            return board.board.Equals(this.board);
        }


        /// <summary>
        /// function that add new tile to the board
        /// </summary>
        public void AddNewTile()
        {
            //find random position for the tile
            TileCoordinates emptyTile = board.GetRandomEmptyTile();

            //put random value in the random chosen tile
            board[emptyTile.Row, emptyTile.Col] = TileValueGenerator.GetTileNumber();

            //update the number of tiles
            board.NumberOfEmptyTiles--;
        }


        /// <summary>
        /// function that determine whether or not the board is full
        /// </summary>
        /// <returns>return true if the board is full else return fale</returns>
        public bool IsFull()
        {
            return board.NumberOfEmptyTiles == 0;
        }

        /// <summary>
        /// function that determine whether or not the board has no more moves
        /// </summary>
        /// <returns>return true if the board lost the game</returns>
        public bool IsLost()
        {
            //check if the board is full
            //if its not the board has more moves
            if (IsFull())
            {
                //if its full
                //check if exists mergeable tiles for each row and column
                //if a marge is possible game not lost

                //check each row
                for (int row = 0; row < RowLength; row++)
                {
                    if (Controller.row_left_table[this.board.GetRowFromRightLeft(row)].NumberOfMergeableTile != 0)
                    {
                        return false;
                    }
                }

                //check each column
                for (int column = 0; column < ColumnLength; column++)
                {
                    if (Controller.row_left_table[this.board.GetColFromDownUp(column)].NumberOfMergeableTile != 0)
                    {
                        return false;
                    }
                }

                //if got to here :
                //the board is full and there is no mergeable tiles
                //meaning lost game
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// shift the whole board to the up
        /// </summary>
        /// <returns>return true if the shifting moved/merged at least one tile</returns>
        public bool ShiftBoardUp()
        {
            return this.board.ShiftBoardUp();
        }

        /// <summary>
        /// shift the whole board to the down
        /// </summary>
        /// <returns>return true if the shifting moved/merged at least one tile</returns>
        public bool ShiftBoardDown()
        {
            return this.board.ShiftBoardDown();
        }

        /// <summary>
        /// shift the whole board to the right
        /// </summary>
        /// <returns>return true if the shifting moved/merged at least one tile</returns>
        public bool ShiftBoardRight()
        {
            return this.board.ShiftBoardRight();
        }

        /// <summary>
        /// shift the whole board to the left
        /// </summary>
        /// <returns>return true if the shifting moved/merged at least one tile</returns>
        public bool ShiftBoardLeft()
        {
            return this.board.ShiftBoardLeft();
        }

        /// <summary>
        /// function that determined the score of the board based on the number of empty tiles 
        /// </summary>
        /// <returns>return the number of empty tiles multiply EvaluateConstants.SCORE_EMPTY_WEIGHT*NumberOfEmptyTiles</returns>
        public double EvaluateEmptyTilesScore()
        {
            return EvaluateConstants.SCORE_EMPTY_WEIGHT * NumberOfEmptyTiles;
        }

        /// <summary>
        /// function that determined the score of the board based on the positioning of the tiles according to
        /// the stratagemMatrix 
        /// </summary>
        /// <param name="stratagemMatrix">Matrix that represent the strategies chosen for the ai </param>
        /// <returns>return the sum of multiplying the value of each tile with the corresponding number
        /// in the stratagemMatrix</returns>
        public double EvaluateTilesPositionsScore(int[,] stratagemMatrix)
        {
            double scoreSum = 0;

            //for every position in the board 
            for (int row = 0; row < Board.ColumnLength; row++)
            {
                for (int col = 0; col < Board.RowLength; col++)
                {
                    //multiply the value of each tile with the corresponding number in the stratagemMatrix
                    scoreSum += stratagemMatrix[row, col] * (1 << this[row, col]);
                }
            }

            return scoreSum;
        }

        /// <summary>
        /// function that determined the score of the board based on the mergeable tiles in it
        /// </summary>
        /// <returns>return the number of mergeable tile multiply by EvaluateConstants.SCORE_MERGES_WEIGHT</returns>
        public double EvaluateMergeableTilesScore()
        {
            double NumberOfMergeableTiles = 0;

            //check number of mergeable tiles for each row
            for (int Row = 0; Row < RowLength; Row++)
            {
                NumberOfMergeableTiles +=
                    Controller.row_left_table[this.board.GetRowFromRightLeft(Row)].NumberOfMergeableTile;
            }

            //check number of mergeable tiles for each column
            for (int Column = 0; Column < ColumnLength; Column++)
            {
                NumberOfMergeableTiles +=
                    Controller.row_left_table[this.board.GetColFromDownUp(Column)].NumberOfMergeableTile;
            }

            return EvaluateConstants.SCORE_MERGES_WEIGHT * NumberOfMergeableTiles;
        }

        /// <summary>
        /// function that determined the score of the board based on if the board reached winning 
        /// </summary>
        /// <returns>return EvaluateConstants.SCORE_GAME_WON if the board reached winning else return 0</returns>
        public double EvaluateWon()
        {
            return IsWon ? EvaluateConstants.SCORE_GAME_WON : 0;
        }

        public uint Score
        {
            get => this.board.Score;
            set => this.board.Score = value;
        }


        public ushort NumberOfEmptyTiles
        {
            get => board.NumberOfEmptyTiles;
            set => board.NumberOfEmptyTiles = value;
        }

        public bool IsWon
        {
            get => board.IsWon;
            set => board.IsWon = value;
        }
    }
}