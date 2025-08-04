using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct PartyGoerData
{
    public string characterName;
    public Sprite bodySprite;
    public Dictionary<ClothingType, ClothingItem> outfit;
}

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;

    public int currentPartyNumber { get; private set; }
    public int totalScore { get; private set; }
    public List<PartyGoerData> partyGoers { get; private set; }
    public int lastRoundScore { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quitting game...");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void RegisterPartyGoers(List<PartyGoerData> allPartyGoerData)
    {
        partyGoers = new List<PartyGoerData>(allPartyGoerData);
        JudgeParty();
    }

    private void JudgeParty()
    {
        lastRoundScore = 0;
        if (partyGoers == null || partyGoers.Count == 0) return;

        PartyGoerData playerData = partyGoers.First(p => p.characterName == "Player");
        var playerOutfit = playerData.outfit;
        List<PartyGoerData> npcData = partyGoers.Where(p => p.characterName != "Player").ToList();

        if (npcData.Count == 0) return;

        lastRoundScore += JudgeSlot(ClothingType.Hat, playerOutfit, npcData);
        lastRoundScore += JudgeSlot(ClothingType.Shirt, playerOutfit, npcData);
        lastRoundScore += JudgeSlot(ClothingType.Pants, playerOutfit, npcData);

        totalScore += lastRoundScore;
        SaveGame();
    }

    private int JudgeSlot(ClothingType type, Dictionary<ClothingType, ClothingItem> playerOutfit, List<PartyGoerData> npcData)
    {
        var voteCounts = npcData
            .Select(npc => npc.outfit.ContainsKey(type) ? npc.outfit[type] : null)
            .Where(item => item != null)
            .GroupBy(item => item)
            .ToDictionary(group => group.Key, group => group.Count());

        if (voteCounts.Count == 0) return 0;
        var winningItem = voteCounts.OrderByDescending(kvp => kvp.Value).First().Key;
        if (playerOutfit.ContainsKey(type) && playerOutfit[type] == winningItem) return 1;
        return 0;
    }

    public void PrepareForNextRound()
    {
        currentPartyNumber++;
        SaveGame();
    }

    public void StartNewGame()
    {
        currentPartyNumber = 0;
        totalScore = 0;
        SaveGame();
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("CurrentPartyNumber", currentPartyNumber);
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        currentPartyNumber = PlayerPrefs.GetInt("CurrentPartyNumber", 0);
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
    }
}