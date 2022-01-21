using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class that responsible on the setting
/// </summary>
public class Settings : MonoBehaviour
{
    public static int SearchDepth = 4;
    public static bool isSnake = false;

    public static bool isAiPlay = true;

    private const int maxAIDepth = 6;
    private const int minAIDepth = 1;

    public Sprite oneSprite;
    public Sprite twoSprite;
    public Sprite threeSprite;
    public Sprite fourSprite;
    public Sprite fiveSprite;
    public Sprite sixSprite;
    private Image AIDepthNumImage;
    private RectTransform checkMarkSwitchToggle;
    private GameObject AISettings;
    private GameObject hideAISettings;
    private Toggle whosPlayingToggle;


    /// <summary>
    /// function that connect between variables to UI elemnts
    /// </summary>
    private void Awake()
    {
        hideAISettings = GameObject.Find("hideAISettings");
        AIDepthNumImage = GameObject.Find("numA.I.Depth").GetComponent<Image>();
        AIDepthNumImage = GameObject.Find("numA.I.Depth").GetComponent<Image>();
        checkMarkSwitchToggle = GameObject.Find("checkmarkWhosPlaying").GetComponent<RectTransform>();
        AISettings = GameObject.Find("AISettings");
        whosPlayingToggle = GameObject.Find("switchWhosePlaying").GetComponent<Toggle>();
    }

    
    /// <summary>
    /// function that updates the setting to be on the current chosen one
    /// </summary>
    void Start()
    {
        AIDepthNumImage.sprite = convertAIDepthToSprite(SearchDepth);
        whosPlaying(isAiPlay);
    }


    /// <summary>
    /// function that decreases the ai depth by 1
    /// </summary>
    public void decreaseA_I_Depth()
    {
        if (SearchDepth > minAIDepth)
        {
            SearchDepth -= 1;
            AIDepthNumImage.sprite = convertAIDepthToSprite(SearchDepth);
        }
    }

    /// <summary>
    /// function that used to save the settings the user chose
    /// </summary>
    public void SaveSettings()
    {
        Toggle[] toggles = GameObject.Find("toggleGroup").GetComponent<ToggleGroup>().GetComponentsInChildren<Toggle>();
        foreach (Toggle t in toggles)
            if (t.isOn)
            {
                if (t.name.Equals("snakeToggle"))
                {
                    isSnake = true;
                }
                else
                {
                    isSnake = false;
                }

                break;
            }
    }

    
    /// <summary>
    /// function that increases the ai depth by 1
    /// </summary>
    public void increaseA_I_Depth()
    {
        if (SearchDepth < maxAIDepth)
        {
            SearchDepth += 1;
            AIDepthNumImage.sprite = convertAIDepthToSprite(SearchDepth);
        }
    }

    /// <summary>
    /// function that connect between the number and its sprite
    /// </summary>
    /// <param name="AIDepthNum">the number of the sprite wanting </param>
    /// <returns>return the corresponding sprite to the number given</returns>
    Sprite convertAIDepthToSprite(int AIDepthNum)
    {
        switch (AIDepthNum)
        {
            case 1: return oneSprite;
            case 2: return twoSprite;
            case 3: return threeSprite;
            case 4: return fourSprite;
            case 5: return fiveSprite;
            case 6: return sixSprite;
            default: return null;
        }
    }

    
    /// <summary>
    /// function that update the screen according to the player type chosen
    /// </summary>
    /// <param name="toggleMode"></param>
    public void whosPlaying(bool toggleMode)
    {
        if (toggleMode)
        {
            hideAISettings.SetActive(false);
            isAiPlay = true;
            float xPosCheckmark = checkMarkSwitchToggle.localPosition.x;
            checkMarkSwitchToggle.localPosition = new Vector2(-xPosCheckmark, checkMarkSwitchToggle.localPosition.y);
            whosPlayingToggle.isOn = true;
        }
        else
        {
            hideAISettings.SetActive(true);
            isAiPlay = false;
            float xPosCheckmark = checkMarkSwitchToggle.localPosition.x;
            checkMarkSwitchToggle.localPosition = new Vector2(-xPosCheckmark, checkMarkSwitchToggle.localPosition.y);
            whosPlayingToggle.isOn = false;
        }
    }
}