using DefaultNamespace;
using OmegaProjectGame;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// thr class that connect the unity and the Control(Ai) 
/// </summary>
public class GameModel : MonoBehaviour
{
    private bool isNeedInput = false;
    private RectTransform lastSwipeArrowRectTransform;
    private Animator lastSwipeArrowAnimator;
    private const float MOVING_TIME = 0.0f;
    private string changeLastSwipeTrigger = "lastSwipeChange";
    private int totalSwipes;
    private Text scoreText;
    private float START_ANIMATION = 1.3f;
    private float timeToTestSimpaleCube = MOVING_TIME;
    private Controller Maincontroller;
    private TileMovement TileMovementManager;
    private UnityController UnityController;
    private movementDirection movementDirection;
    private Text totalSwipeText;
    private Tile[] UpdateableTiles;
    private BoardModel boardModel;


    
    /// <summary>
    /// function that initialize objects and connect  variables to  Ui element   
    /// </summary>
    private void Awake()
    {
        TileMovementManager = GameObject.FindGameObjectWithTag("Cubes").GetComponent<TileMovement>();
        movementDirection = GameObject.Find("movmentControl").GetComponent<movementDirection>();
        UnityController = GameObject.Find("backgroundTransition").GetComponent<UnityController>();
        Maincontroller = Controller.CreateController;
        Maincontroller.RunGame();
        lastSwipeArrowRectTransform = GameObject.Find("arrowLastSwipeImage").GetComponent<RectTransform>();
        lastSwipeArrowAnimator = GameObject.Find("arrowLastSwipeImage").GetComponent<Animator>();
        totalSwipeText = GameObject.Find("amountOfSwipesText").GetComponent<Text>();
        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        START_ANIMATION = 1.3f;
    }


    /// <summary>
    /// function that update the ui board at the start of the game
    /// after the 2 random tile has been placed
    /// </summary>
    /// <param name="board">the board that the tiles had been placed on</param>
    public void StartGame(Board board)
    {
        UpdateableTiles = new Tile[16];
        boardModel = new BoardModel();

        int TilesCounter = 0;

        //finding the tiles needed to be updated in the ui
        for (int row = 0; row < Board.ColumnLength; row++)
        {
            for (int col = 0; col < Board.RowLength; col++)
            {
                UpdateableTiles[TilesCounter] = new Tile(row, col, board[row, col]);
                TilesCounter++;
            }
        }

        //initializing the ui board with the tiles found
        boardModel.initBoard(UpdateableTiles);

        //update the board according to the boardModel
        updateUiBoard(Directions.NoMove, board.Score, boardModel);
    }

    
    /// <summary>
    /// function that update the board in the ui to the board given
    /// </summary>
    /// <param name="board">the board to be represented in the ui</param>
    /// <param name="direction">the direction of the last shifft</param>
    public void updateBoard(Board board, Directions direction)
    {
        int TilesCounter = 0;

        //finding the tiles needed to be updated in the ui
        for (int row = 0; row < Board.ColumnLength; row++)
        {
            for (int col = 0; col < Board.RowLength; col++)
            {
                UpdateableTiles[TilesCounter] = new Tile(row, col, board[row, col]);
                TilesCounter++;
            }
        }


        //initializing the ui board with the tiles found
        boardModel.initBoard(UpdateableTiles);
        updateUiBoard(direction, board.Score, boardModel);
    }
    
    /// <summary>
    /// function that update the board , the AI-LastSwipe, the number of swipes and the score
    /// </summary>
    /// <param name="direction">the direction of the last swipe</param>
    /// <param name="boardScore">the new board score</param>
    /// <param name="board">the new board</param>
    public void updateUiBoard(Directions direction, uint boardScore, BoardModel board)
    {
        TileMovementManager.UpdateBoard(board);
        switch (direction)
        {
            case Directions.Up:
                scoreText.text = boardScore.ToString();
                newSwipeHappened(0);
                break;
            case Directions.Down:
                scoreText.text = boardScore.ToString();
                newSwipeHappened(180);
                break;
            case Directions.Left:
                scoreText.text = boardScore.ToString();
                newSwipeHappened(90);
                break;
            case Directions.Right:
                scoreText.text = boardScore.ToString();
                newSwipeHappened(270);
                break;
            case Directions.NoMove:
                break;
        }
    }

    /// <summary>
    /// function that update the ai last swipe
    /// </summary>
    /// <param name="arrowRotation">the rotation of the last swipr</param>
    private void newSwipeHappened(int arrowRotation)
    {
        lastSwipeArrowRectTransform.eulerAngles = new Vector3(0, 0, arrowRotation);
        lastSwipeArrowAnimator.SetTrigger(changeLastSwipeTrigger);
        totalSwipes += 1;
        totalSwipeText.text = totalSwipes.ToString();
    }


    /// <summary>
    /// function that calls the controller when the ai plays
    /// </summary>
    private void AiPlay()
    {
        if (!Maincontroller.isGameFinished())
        {
            Maincontroller.RunGame();
        }
        else
        {
            //if file mode is turned on 
            //the board will be written in outside fil
            //and a new game will start
            if (!Controller.FileMode)
            {
                if (!Maincontroller.isWon())
                {
                    UnityController.MoveToGameOver();
                }
                else
                {
                    UnityController.moveToGameWon();
                }
            }
            else
            {
                Maincontroller.GameEnded();
                UnityController.reastart();
            }
        }
    }


    
    /// <summary>
    /// function that calls the controller when the user plays
    /// </summary>
    private void HumanPlay()
    {
        if (!isNeedInput)
        {
            if (!Maincontroller.isGameFinished())
            {
                Maincontroller.RunGame();
            }
            else
            {
                if (!Maincontroller.isWon())
                {
                    UnityController.MoveToGameOver();
                }
                else
                {
                    UnityController.moveToGameWon();
                }
            }
        }
        else
        {
            if (isNeedInput && movementDirection.IsInputGiven)
            {
                if (!Maincontroller.isGameFinished())
                {
                    Maincontroller.RunGame();
                }
                else
                {
                    if (!Maincontroller.isWon())
                    {
                        UnityController.MoveToGameOver();
                    }
                    else
                    {
                        UnityController.moveToGameWon();
                    }
                }
            }
        }
    }

   

    /// <summary>
    /// function that calls the control to run the game
    /// </summary>
    void Update()
    {
        START_ANIMATION -= Time.deltaTime;
        if (START_ANIMATION <= 0)
        {
            if (!UnityController.isGamePaused)
            {
                timeToTestSimpaleCube -= Time.deltaTime;
                if (timeToTestSimpaleCube <= 0)
                {
                    if (Settings.isAiPlay)
                    {
                        AiPlay();
                    }
                    else
                    {
                        HumanPlay();
                    }

                    timeToTestSimpaleCube = MOVING_TIME;
                }
            }
        }
    }

    /// <summary>
    /// function  that gets the direction the user submitted
    /// </summary>
    /// <returns>return the direction the user submitted</returns>
    public Directions GetUserDirection()
    {
        return movementDirection.GetUserDirection();
    }

    /// <summary>
    /// function that indicate if the controller can run or wait for input
    /// </summary>
    /// <returns>returm true if the controller can run</returns>
    public bool CanRun()
    {
        return (!isNeedInput || isNeedInput && IsInputGiven);
    }
    
    public bool IsInputGiven
    {
        get => movementDirection.IsInputGiven;
        set => movementDirection.IsInputGiven = value;
    }

    public bool IsNeedInput
    {
        get => isNeedInput;
        set => isNeedInput = value;
    }
}