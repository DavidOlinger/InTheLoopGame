// Located at: Assets/Scripts/UIScripts/TitleMenu.cs
// FINAL VERSION FOR PART 1

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public string mainGameSceneName = "Level"; // Corrected scene name
    public string controlsSceneName = "Controls";

    void Start()
    {
        // Ensure the PartyManager exists and tell it to reset progress.
        if (PartyManager.instance == null)
        {
            // This will create the manager and load any saved data.
            new GameObject("PartyManager").AddComponent<PartyManager>();
        }
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