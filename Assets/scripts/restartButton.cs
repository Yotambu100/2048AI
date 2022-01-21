using UnityEngine;

/// <summary>
/// class that mange the restart button
/// </summary>
public class restartButton : MonoBehaviour
{
    private UnityController unityController;
    
    /// <summary>
    /// connects the variable unityController to the class UnityController
    /// </summary>
    private void Awake()
    {
        unityController = GameObject.Find("controler").GetComponent<UnityController>();
    }
    
    
    /// <summary>
    /// function that indicate that the restart button is pressed
    /// </summary>
    private void OnMouseDown()
    {
        unityController.IsPlayerWantToRestart = true;
    }
    
    /// <summary>
    /// function that indicate that the restart button is not pressed
    /// </summary>
    private void OnMouseUp()
    {
        unityController.IsPlayerWantToRestart = false;
    }


}
