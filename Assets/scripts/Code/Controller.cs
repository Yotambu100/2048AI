using System;
using DefaultNamespace.GameScripts;
using UnityEngine;

namespace OmegaProjectGame
{
    /// <summary>
    /// enum that represent the current game status
    /// </summary>
    public enum GameStatus
    {
        InitializeNewGame,
        GetMove,
        DoMove,
        FinishGame,
        AddNewTile,
        CheckIfLost
    }

    /// <summary>
    /// class that manages the flow of the game part by part
    /// </summary>
    public class Controller
    {
        //singleton object
        private static Controller controllerInstance = null;

        //the lookup table of shifted rows
        public static ShiftedRow[] row_left_table = new ShiftedRow[65536];
        
        //bool that enable file mode
        //meaning the game will run in an endless loop and every time the ai will lose
        //it will write the board at losing state and restart a new game
        public static Boolean FileMode = false;

        
        private GameStatus gameStatus;
        // private readonly GameView gameView;
        private Board board;
        private Directions direction;
        private readonly Model model;
        private FileResults fileResults;
        
        //gameView that connect the ui to the controller
        private static GameModel gameView;


        
        /// <summary>
        /// private constructor that initialize some of his variable
        /// this function will be called only one time because the controller is a singleton object 
        /// </summary>
        private Controller()
        {
            gameStatus = GameStatus.InitializeNewGame;
            init_tables();
            model = new Model();
            fileResults = new FileResults();
        }

        
        /// <summary>
        /// function that return controller
        /// if controller was already made it will return the already created one if not it will create a new controller
        /// and return if
        /// </summary>
        /// <returns>return the controller singleton</returns>
        public static Controller CreateController
        {
            get
            {
                // if a controller was not made
                if (controllerInstance == null)
                {
                    //create a new one 
                    controllerInstance = new Controller();
                }
                
                // connect betwenn the controler
                gameView= GameObject.FindGameObjectWithTag("gameModel").GetComponent<GameModel>();
                controllerInstance.gameStatus = GameStatus.InitializeNewGame;
                return controllerInstance;
            }
        }


        /// <summary>
        /// The function manages the whole game part by part
        /// according to the current gameStatus
        /// </summary>
        public void RunGame()
        {
          
            bool isPrinted = false;

            // doing part by part until you need to update the board or the game is finished
            while (!isPrinted && gameStatus != GameStatus.FinishGame && gameView.CanRun())
            {
                switch (gameStatus)
                {
                    // Initializing all the object
                    case GameStatus.InitializeNewGame:
                        model.InitializeModel(Settings.isSnake);
                        board = new Board();
                        board.InitializeBoard();
                        gameView.StartGame(board);
                        isPrinted = true;
                        gameStatus = GameStatus.GetMove;
                        gameView.IsNeedInput = !Settings.isAiPlay;
                        break;

                    //get the next move from the player/ai
                    case GameStatus.GetMove:
                        direction = Settings.isAiPlay ? model.AiGetBestMove(board) : gameView.GetUserDirection();
                        gameView.IsNeedInput = false;
                        gameStatus = direction == Directions.NoMove ? GameStatus.FinishGame : GameStatus.DoMove;
                        break;

                    //do move
                    case GameStatus.DoMove:
                        //if the move was valid
                        if (model.DoMove(board, direction))
                        {
                            // gameView.PrintBoard(board);
                            gameView.updateBoard(board, direction);
                            isPrinted = true;
                            gameStatus = board.IsWon ? GameStatus.FinishGame : GameStatus.AddNewTile;
                        }
                        else
                        {
                            //if the move wasn't valid
                            //get anther move
                            gameView.IsNeedInput = !Settings.isAiPlay;
                            gameStatus = GameStatus.GetMove;
                        }

                        break;

                    //add new tile to the board
                    case GameStatus.AddNewTile:
                        board.AddNewTile();
                        // gameView.PrintBoard(board);
                        gameView.updateBoard(board, Directions.NoMove);
                        isPrinted = true;
                        gameStatus = GameStatus.CheckIfLost;
                        break;

                    //check if lost
                    case GameStatus.CheckIfLost:
                        if (Settings.isAiPlay || !model.DidLost(board))
                        {
                            gameView.IsNeedInput = !Settings.isAiPlay;
                            gameStatus = GameStatus.GetMove;
                        }
                        else
                        {
                            gameStatus = GameStatus.FinishGame;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// THe function determine if the game is over 
        /// </summary>
        /// <returns>True if game lost or won</returns>
        public bool isGameFinished()
        {
            return gameStatus == GameStatus.FinishGame;
        }

        public bool isWon()
        {
            return board.IsWon;
        }

        /// <summary>
        /// the function load an array of shifting row
        /// the index is the original row and the value is the new shifted left row(with some additional information)
        /// </summary>
        public void init_tables()
        {
            //the original row
            byte[] line = new Byte[4];

            //the number of Mergeable tiles in row
            ushort numberOfMergeableTile;

            //the number of empty tiles in row
            ushort numberOfEmptyTile = 0;

            //bool that hold if the row achieved meaning game won(cent get bigger tile mathematically)
            bool isWon;


            uint score;
            //for every row possible
            for (int row = 0; row < 65536; ++row)
            {
                numberOfMergeableTile = 0;
                numberOfEmptyTile = 0;
                isWon = false;

                line[0] = (byte) ((row >> 0) & 0xf);
                line[1] = (byte) ((row >> 4) & 0xf);
                line[2] = (byte) ((row >> 8) & 0xf);
                line[3] = (byte) ((row >> 12) & 0xf);


                //calculating the number of empty spaces in the original row
                for (int i = 0; i < Board.RowLength; i++)
                {
                    if (line[i] == 0)
                    {
                        numberOfEmptyTile++;
                    }
                }

                // calculating the score
                score = 0;
                for (int i = 0; i < 4; ++i)
                {
                    int rank = line[i];
                    if (rank >= 2)
                    {
                        //calculating the score needed to achieve every tile in row
                        score += (uint) ((rank - 1) * (1 << rank));
                    }
                }

                // execute a move to the left
                for (int i = 0; i < 3; ++i)
                {
                    int j;
                    for (j = i + 1; j < 4; ++j)
                    {
                        if (line[j] != 0) break;
                    }

                    if (j == 4) break; // no more tiles to the right

                    if (line[i] == 0)
                    {
                        line[i] = line[j];
                        line[j] = 0;
                        i--; // retry this entry
                    }
                    else if (line[i] == line[j])
                    {
                        if (line[i] != 0xf)
                        {
                            //marge two tiles
                            numberOfMergeableTile++;
                            line[i]++;
                        }
                        else
                        {
                            isWon = true;
                        }

                        line[j] = 0;
                    }
                }


                //the new shifted row
                ushort result = (ushort) (line[0] << 0);
                result |= (ushort) (line[1] << 4);
                result |= (ushort) (line[2] << 8);
                result |= (ushort) (line[3] << 12);

                //save all the information in the array in the correct position
                //(the new row is saved as row^result in order to do the conversation easily)
                row_left_table[row] = new ShiftedRow(numberOfEmptyTile, (ushort) (row ^ result), numberOfMergeableTile,
                    score, isWon);
            }
        }


        /// <summary>
        /// function that used to document the finle board into file in order to test the best evaluate functions and numbers
        /// </summary>
        public void GameEnded()
        {
            fileResults.WriteBoardResultOnFile(this.board);
        }
    }
}