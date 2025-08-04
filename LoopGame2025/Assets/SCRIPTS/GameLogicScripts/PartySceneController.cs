using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
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
    public float timeBetweenReveals = 0.5f;
    public float delayAfterLastReveal = 1.5f;

    void Start()
    {
        if (PartyManager.instance == null)
        {
            Debug.LogError("PartyManager not found! Cannot display party results. Returning to Title.");
            SceneManager.LoadScene("TitleScene");
            return;
        }
        resultsPanel.SetActive(false);
        StartCoroutine(RevealSequence());
    }

    private IEnumerator RevealSequence()
    {
        List<PartyGoerData> partyGoers = PartyManager.instance.partyGoers;

        // Reveal each partygoer's portrait one by one
        foreach (var partyGoer in partyGoers)
        {
            GameObject portraitGO = Instantiate(partyPortraitPrefab, portraitGrid);
            PortraitUI portraitUI = portraitGO.GetComponent<PortraitUI>();
            if (portraitUI != null)
            {
                var outfitList = new List<ClothingItem>(partyGoer.outfit.Values);
                portraitUI.DisplayOutfit(partyGoer.bodySprite, outfitList, partyGoer.characterName);
            }

            yield return new WaitForSeconds(timeBetweenReveals);
        }

        // Wait a moment after the last reveal for dramatic effect
        yield return new WaitForSeconds(delayAfterLastReveal);

        // Show the results panel
        resultsPanel.SetActive(true);
        scoreText.text = $"Your Score This Round: {PartyManager.instance.lastRoundScore} / 3";
    }

    public void GoToNext()
    {
        // Check if this was the final party
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