using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public string mainGameSceneName = "Level";
    public string controlsSceneName = "Controls";

    void Start()
    {
        if (PartyManager.instance == null)
        {
            // Create the PartyManager if it doesn't exist yet.
            // This happens when the game is first launched.
            new GameObject("PartyManager").AddComponent<PartyManager>();
        }
        // Always reset the game state when returning to the title screen.
        PartyManager.instance.StartNewGame();
    }

    public void GoToMainLevel()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }

    public void GoToControls()
    {
        SceneManager.LoadScene(controlsSceneName);
    }
}