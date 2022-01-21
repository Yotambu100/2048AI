using UnityEngine;

/// <summary>
/// class that responsible about the courser
/// </summary>
public class customCursor : MonoBehaviour
{
    public movementDirection movementDirectionScript;
    public Texture2D texture2DMouseRegular;
    public Texture2D texture2DMouseSlide;
    
    /// <summary>
    /// function that set the custom courser on the mouse
    /// </summary>
    void Start()
    {
        Cursor.SetCursor(texture2DMouseRegular, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// function that manage the courser when he is clicked
    /// </summary>
    void Update()
    {
        if(movementDirectionScript.isMouseClicking)
        {
            Cursor.SetCursor(texture2DMouseSlide, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(texture2DMouseRegular, Vector2.zero, CursorMode.Auto);
        }
    }
}
