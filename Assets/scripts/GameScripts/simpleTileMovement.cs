using UnityEngine;


/// <summary>
/// class that responsible on tile movement
/// </summary>
public class simpleTileMovement : MonoBehaviour
{
    private int row;
    private int column;

    
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }



    /// <summary>
    /// function that set the tile position at his row and column
    /// </summary>
    void Start()
    {
        GameObject tiles =  GameObject.Find("tiles");
        transform.SetParent(tiles.transform);
        transform.localPosition = new Vector2(convertColumnToXAxis(), convertRowToYAxis());
        transform.localScale = Vector3.one;
    }

    
    /// <summary>
    /// function that convert the row to the correct Y location
    /// </summary>
    /// <returns>return the actual Y position</returns>
    float convertRowToYAxis()
    {
        switch (row)
        {
            case 0: return 0f;
            case 1: return -2.4f;
            case 2: return -4.7f;
            case 3: return -7f;
            case 4: return -9.3f;
        }
        return 0;
    }
    
    /// <summary>
    /// function that convert the column to the correct X location
    /// </summary>
    /// <returns>return the actual X position</returns>
    float convertColumnToXAxis()
    {
        switch (column)
        {
            case 0: return 0f;
            case 1: return 2.4f;
            case 2: return 4.8f;
            case 3: return 7.2f;
            case 4: return 9.6f;
        }
        return 0;
    }
}
