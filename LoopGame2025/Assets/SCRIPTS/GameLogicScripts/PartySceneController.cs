// Located at: Assets/Scripts/GameLogicScripts/PartySceneController.cs
// FINAL VERSION WITH SEQUENTIAL REVEAL

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; // Added
using TMPro;

public class PartySceneController : MonoBehaviour
{
    [Header("Scene Setup")]
    public Transform portraitGrid;
    public GameObject partyPortraitPrefab;

    [Header("Results UI")]
    public GameObject resultsPanel;
    public TextMeshProUGUI scoreText;

    [Header("Reveal Animation")]
    public float timeBetweenReveals = 0.5f; // Time between each portrait appearing
    public float delayAfterLastReveal = 1.5f; // Time after the last portrait until the score appears

    void Start()
    {
        if (PartyManager.instance == null) { return; }
        resultsPanel.SetActive(false);
        StartCoroutine(RevealSequence()); // Start the main reveal sequence
    }

    private IEnumerator RevealSequence()
    {
        // Get the list of party goers
        List<PartyGoerData> partyGoers = PartyManager.instance.partyGoers;

        // Loop through and reveal each one
        foreach (var partyGoer in partyGoers)
        {
            GameObject portraitGO = Instantiate(partyPortraitPrefab, portraitGrid);
            PortraitUI portraitUI = portraitGO.GetComponent<PortraitUI>();
            if (portraitUI != null)
            {
                var outfitList = new List<ClothingItem>(partyGoer.outfit.Values);
                portraitUI.DisplayOutfit(partyGoer.bodySprite, outfitList, partyGoer.characterName);
            }

            // Wait before revealing the next one
            yield return new WaitForSeconds(timeBetweenReveals);
        }

        // Wait after the final portrait has been revealed
        yield return new WaitForSeconds(delayAfterLastReveal);

        // Now, show the results panel
        resultsPanel.SetActive(true);
        scoreText.text = $"Your Score This Round: {PartyManager.instance.lastRoundScore} / 3";
    }

    public void GoToNext()
    {
        if (PartyManager.instance.currentPartyNumber >= 3)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            SceneManager.LoadScene("Level");
        }
    }
}