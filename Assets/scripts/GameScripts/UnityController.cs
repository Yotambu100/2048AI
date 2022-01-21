using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// class that control and manages all the unity
/// </summary>
public class UnityController : MonoBehaviour
{
    private Animator transitionAnimator;
    private Animator gameOverMessageAnimator;
    private Animator gameWonMessageAnimator;
    private float timeThePlayerWantToRestart = 0;
    private Image backgroundRestartImage;
    private bool isPlayerWantToRestart = false;
    private Animator mainCameraAnimator;
    private string seanceNameToLoad;
    private UnityController unityControllerScript;
    private bool inPlaySeance;
    public bool isGamePaused = false;
    public Sprite pouseSprite;
    public Sprite playSprite;
    private Image pousePlayButtonImage;
    private RectTransform pousePlayButtonRectTransform;

    
    /// <summary>
    /// function that connect the variables to other Ui element
    /// </summary>
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "playGameSence")
        {
            inPlaySeance = true;
            gameOverMessageAnimator = GameObject.Find("gameOverMessage").GetComponent<Animator>();
            backgroundRestartImage = GameObject.Find("backgroundRestart").GetComponent<Image>();
            mainCameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
            pousePlayButtonImage = GameObject.Find("pouse/playButton").GetComponent<Image>();
            pousePlayButtonRectTransform = GameObject.Find("pouse/playButton").GetComponent<RectTransform>();
            gameWonMessageAnimator = GameObject.Find("gameWonMessage").GetComponent<Animator>();
        }

        transitionAnimator = GameObject.Find("backgroundTransition").GetComponent<Animator>();
        unityControllerScript = GameObject.Find("backgroundTransition").GetComponent<UnityController>();
    }


    /// <summary>
    /// function that read the player input and act acordingly
    /// </summary>
    void Update()
    {
        if (inPlaySeance)
        {
            if (Input.GetKeyDown(KeyCode.P) && transform.name == "backgroundTransition")
            {
                pauseOrPlayGame();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                playTransition("startSence");
            }

            if (Input.GetKey(KeyCode.R))
            {
                decreaseRestartValue();
                if (timeThePlayerWantToRestart > 1)
                {
                    reastart();
                }
            }
            else if (isPlayerWantToRestart)
            {
                decreaseRestartValue();
                if (timeThePlayerWantToRestart > 1)
                {
                    reastart();
                }
            }
            else
            {
                timeThePlayerWantToRestart = 0;
                backgroundRestartImage.fillAmount = timeThePlayerWantToRestart;
            }
        }
    }

    /// <summary>
    /// function that pause and unpause the game according to current status
    /// </summary>
    public void pauseOrPlayGame()
    {
        if (unityControllerScript.isGamePaused)
        {
            pousePlayButtonRectTransform.localScale = new Vector3(0.7f, 0.7f, 1);
            unityControllerScript.isGamePaused = false;
            pousePlayButtonImage.sprite = pouseSprite;
        }
        else
        {
            pousePlayButtonRectTransform.localScale = new Vector3(0.8f, 0.7f, 1);
            pousePlayButtonImage.sprite = playSprite;
            unityControllerScript.isGamePaused = true;
        }
    }

    /// <summary>
    /// function that indicate that the  user wants to restart
    /// </summary>
    public void playerWantToRestart()
    {
        isPlayerWantToRestart = true;
    }

    /// <summary>
    /// function that count the time the user wanted to restart
    /// </summary>
    private void decreaseRestartValue()
    {
        timeThePlayerWantToRestart += Time.deltaTime;
        backgroundRestartImage.fillAmount = timeThePlayerWantToRestart;
    }
    
    /// <summary>
    /// function that open game over
    /// </summary>
    public void MoveToGameOver()
    {
        mainCameraAnimator.SetTrigger("shakeCamera");
        gameOverMessageAnimator.SetTrigger("gameEnd");
    }

    /// <summary>
    /// function that restart the game
    /// </summary>
    public void reastart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    /// <summary>
    /// function that load new seance according to the seanceNameToLoad
    /// </summary>
    public void loadSence()
    {
        SceneManager.LoadScene(seanceNameToLoad);
    }

    /// <summary>
    /// function that play transition and load seance according to the senceName
    /// </summary>
    /// <param name="senceName">the name of the seance to be loaded</param>
    public void playTransition(string senceName)
    {
        unityControllerScript.seanceNameToLoad = senceName;
        transitionAnimator.SetTrigger("loadSence");
    }

  
    /// <summary>
    /// function that open game won
    /// </summary>
    public void moveToGameWon()
    {
        mainCameraAnimator.SetTrigger("shakeCamera");
        gameWonMessageAnimator.SetTrigger("gameEnd");
    }
    
      

    public bool IsPlayerWantToRestart
    {
        get => isPlayerWantToRestart;
        set => isPlayerWantToRestart = value;
    }

}