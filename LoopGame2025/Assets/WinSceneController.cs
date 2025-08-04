// Located at: Assets/Scripts/UIScripts/WinSceneController.cs
// NEW SCRIPT

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinSceneController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        if (PartyManager.instance != null)
        {
            finalScoreText.text = $"You attended all the parties!\nYour final score: {PartyManager.instance.totalScore} / 9";
        }
        else
        {
            finalScoreText.text = "Error: Score data not found.";
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}