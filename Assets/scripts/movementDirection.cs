using OmegaProjectGame;
using UnityEngine;

public class movementDirection : MonoBehaviour
{
    public bool isMouseClicking;
    public Directions moveDirecton;
    private Vector2 startClickPosition;
    private Vector2 endClickPosition;
    private bool isInputGiven;

    /// <summary>
    /// function that initializing its variables
    /// </summary>
    void Start()
    {
        isMouseClicking = false;
        isInputGiven = false;
    }

    /// <summary>
    /// function that get the user input and convert it to diraction
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirecton = Directions.Up;
            Debug.Log("up arrow click");
            isInputGiven = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirecton = Directions.Down;
            isInputGiven = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirecton = Directions.Right;
            isInputGiven = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirecton = Directions.Left;
            isInputGiven = true;
        }
        else
        {
            // isInputGiven = false;
        }

        if (Input.GetMouseButtonUp(0) && isMouseClicking)
        {
            endClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            swipeDirections();
            isMouseClicking = false;
            isInputGiven = true;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            startClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMouseClicking = true;
        }
        else
        {
            // isInputGiven = false;
        }
    }

    /// <summary>
    /// function that determine in which way the user swiped
    /// </summary>
    void swipeDirections()
    {
        int movingHorizontal;
        int movingVertical;
        float differenceX = startClickPosition.x - endClickPosition.x;
        if (differenceX < 0)
        {
            differenceX *= -1;
            movingHorizontal = 1;
        }
        else
        {
            movingHorizontal = -1;
        }

        float differenceY = startClickPosition.y - endClickPosition.y;
        if (differenceY < 0)
        {
            differenceY *= -1;
            movingVertical = 1;
        }
        else
        {
            movingVertical = -1;
        }

        if (differenceX > differenceY)
        {
            if (movingHorizontal == 1)
            {
                moveDirecton = Directions.Right;
            }
            else if (movingHorizontal == -1)
            {
                moveDirecton = Directions.Left;
            }
        }
        else
        {
            if (movingVertical == 1)
            {
                moveDirecton = Directions.Up;
            }
            else if (movingVertical == -1)
            {
                moveDirecton = Directions.Down;
            }
        }
    }
    
    /// <summary>
    /// function that gets the direction given by the user
    /// and update the input status
    /// </summary>
    /// <returns>return the user input</returns>
    public Directions GetUserDirection()
    {
        isInputGiven = false;
        return moveDirecton;
    }

    public bool IsInputGiven
    {
        get => isInputGiven;
        set => isInputGiven = value;
    }

    
}