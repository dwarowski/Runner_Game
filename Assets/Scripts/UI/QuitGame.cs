using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For stopping play mode in the editor
#else
        Application.Quit(); // For quitting the application in a built game
#endif
    }
}
