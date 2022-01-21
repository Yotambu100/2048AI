using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmegaProjectGame
{
    /// <summary>
    /// Class that hold the Constant number for all the evaluation
    /// </summary>
    public class EvaluateConstants
    {
        public const double SCORE_EMPTY_WEIGHT = 128.0f;

        public const double SCORE_MERGES_WEIGHT = 256.0f;
        public const double SCORE_GAME_OVER = -5000000.0f;
        public const double SCORE_GAME_WON = 5000000.0f;
                                             

        public static int[,] SnakeMatrix =
        {
            {1024, 512, 256, 128},
            {8, 16, 32, 64},
            {4, 2, 2, 0},
            {0, 0, 0, 0}
        };

        public static int[,] CornerGrid =
        {
            {4096, 1024, 256, 64},
            {1024, 256, 64, 16},
            {256, 64, 16, 4},
            {64, 16, 4, 1}
        };
    }

    /// <summary>
    /// Class that manages all the movement the calculations
    /// </summary>
    public class Model
    {
        //the four possible movement directions  
        private Directions[] allMoves = new Directions[]
            {Directions.Up, Directions.Down, Directions.Left, Directions.Right};

        //The transposition table
        private Dictionary<ulong, TranspositionValue> TranspositionTable;

        //The transposition table capacity
        private int TranspositionTableCapacity = 2000000;


        //the matrix that represent the strategy chosen by the user
        private int[,] StratagemMatrix;

        /// <summary>
        /// constructor
        /// </summary>
        public Model()
        {
        }

        /// <summary>
        /// Function that initializing the model
        /// </summary>
        /// <param name="isSnake">bool that represent which strategy the user chose if true Snake strategy
        /// else corner strategy</param>
        public void InitializeModel(bool isSnake)
        {
            if (isSnake)
            {
                StratagemMatrix = EvaluateConstants.SnakeMatrix;
            }
            else
            {
                StratagemMatrix = EvaluateConstants.CornerGrid;
            }
        }


        /// <summary>
        /// function that preform a movement to the direction given on the board given
        /// </summary>
        /// <param name="board">the board the movement is being done on</param>
        /// <param name="direction">the direction of the movement </param>
        /// <returns></returns>
        public bool DoMove(Board board, Directions direction)
        {
            //base on the direction given call the corresponding shifting function
            switch (direction)
            {
                case Directions.Up:
                    return board.ShiftBoardUp();
                    break;
                case Directions.Right:
                    return board.ShiftBoardRight();
                    break;
                case Directions.Down:
                    return board.ShiftBoardDown();
                    break;
                case Directions.Left:
                    return board.ShiftBoardLeft();
                    break;
            }

            throw new Exception("move");
        }


        /// <summary>
        /// function that determine whether or not the board has no more moves
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool DidLost(Board board)
        {
            return board.IsLost();
        }


        /// <summary>
        /// The function that start the Ai
        /// </summary>
        /// <param name="board">the board that the ai will search the best move on</param>
        /// <returns>return the direction of the best mov</returns>
        public Directions AiGetBestMove(Board board)
        {
            Directions bestDirection = Directions.NoMove;
            double bestScore = double.MinValue;
            double score;

            //initializing the TranspositionTable
            TranspositionTable = new Dictionary<ulong, TranspositionValue>(TranspositionTableCapacity);

            //for every direction
            foreach (Directions direction in allMoves)
            {
                //create a backup board
                Board testBoard = board.CreateBackUpBoard();

                //preform the move on the backup board
                DoMove(testBoard, direction);

                //if equal the move didnt have any change so stop
                if (!board.Equals(testBoard))
                {
                    //call the recursive 
                    score = generateScore(testBoard, 0, adaptSearchDepth(testBoard) - 1);

                    //if the current score is bigger than best score save the current score as best and the move as best move
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestDirection = direction;
                    }
                }
            }

            return bestDirection;
        }

        /// <summary>
        /// function that adapt the search depth based on the number of empty tiles
        /// </summary>
        /// <param name="board">the board the search will be on</param>
        /// <returns>return the adapted search depth</returns>
        private int adaptSearchDepth(Board board)
        {
            if (board.NumberOfEmptyTiles > 4)
            {
                return Settings.SearchDepth;
            }

            return Settings.SearchDepth + 1;
        }


        /// <summary>
        /// the function calculate the optimal average score based on the possible positions of tiles spawning and their values 
        /// </summary>
        /// <param name="board">the board the function simulate the spawning of tiles</param>
        /// <param name="currentDepth">the current depth of the algorithm</param>
        /// <param name="searchDepth">the max depth of the algorithm</param>
        /// <returns>return the optimal average score</returns>
        private double generateScore(Board board, int currentDepth, int searchDepth)
        {
            TranspositionValue value;

            //if there is a value in the TranspositionTable for the unique board state and the depth of the
            //value is smaller or equal(meaning that the algorithm already check identical board with depth at least
            //equal) stop searching and return the score in the TranspositionTable
            if (TranspositionTable.TryGetValue(board.GetBoardKey(), out value) && value.Depth <= currentDepth) return value.Score;

            //if  reached maximum depth or board won 
            //finish search and return the evaluation of the board 
            if (currentDepth == searchDepth || board.IsWon) return calculatedFinaleScore(board);

            double totalScore = 1, score2, score4;
            int col;

            //placing 2/4 in every empty spot
            board.NumberOfEmptyTiles--;
            for (int row = 0; row < Board.RowLength; row++)
            {
                for (col = 0; col < Board.ColumnLength; col++)
                {
                    //find empty space
                    if (board[row, col] == 0)
                    {
                        //put 2 in tile
                        board[row, col] = 1;

                        //find the score in case a 2 spawn in that location
                        score2 = CalculateMoveScore(board, currentDepth, searchDepth);

                        // multiply the score by the actual chances of 2 spawning their
                        totalScore += 0.9 * score2;

                        //put 4 in tile
                        board[row, col] = 2;

                        //find the score in case a 4 spawn in that location
                        score4 = CalculateMoveScore(board, currentDepth, searchDepth);

                        // multiply the score by the actual chances of 4 spawning their
                        totalScore += 0.1 * score4;

                        board[row, col] = 0;
                    }
                }
            }

            board.NumberOfEmptyTiles++;

            //calculating the average possible score
            totalScore /= board.NumberOfEmptyTiles;

            //add the current value to the TranspositionTable
            value =
                new TranspositionValue(totalScore, (ushort) currentDepth);
            TranspositionTable[board.GetBoardKey()] = value;


            return totalScore;
        }

        /// <summary>
        /// The function find the score of the best move statistically  
        /// </summary>
        /// <param name="board">the board the function simulate ta move on</param>
        /// <param name="currentDepth">the current depth of the algorithm</param>
        /// <param name="searchDepth">the max depth of the algorithm</param>
        /// <returns>return score of the best move statistically</returns>
        private double CalculateMoveScore(Board board, int currentDepth, int searchDepth)
        {
            double bestScore = EvaluateConstants.SCORE_GAME_OVER;
            double score;
            Board testBoard;

            //for each possible move
            foreach (Directions direction in allMoves)
            {
                //create a backup board
                testBoard = board.CreateBackUpBoard();
                
                //preform the move on the backup board
                DoMove(testBoard, direction);
                
                //if equal the move didnt have any change so stop
                if (!testBoard.Equals(board))
                {
                    
                    //call the recursive  with depth bigger by one
                    score = generateScore(testBoard, currentDepth + 1, searchDepth);
                    
                    //if the current score is bigger than best score save the current score as
                    //best
                    bestScore = score > bestScore ? score : bestScore;
                }
            }

            return bestScore;
        }

        /// <summary>
        /// function that evaluate a given board  
        /// </summary>
        /// <param name="board">the board to be evaluated</param>
        /// <returns>return the score of the board</returns>
        private double calculatedFinaleScore(Board board)
        {
            double score = 0;
            score += board.EvaluateTilesPositionsScore(this.StratagemMatrix);
            score += board.EvaluateEmptyTilesScore();
            score += board.EvaluateMergeableTilesScore();
            score += board.EvaluateWon();
            return score;
        }
    }
}
