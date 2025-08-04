using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState { Investigation, PartyTime }
    public GameState CurrentState { get; private set; }

    [Header("All Clothing Items In The Game")]
    public List<ClothingItem> allHats;
    public List<ClothingItem> allShirts;
    public List<ClothingItem> allPants;

    public ClothingItem HotHat { get; private set; }
    public ClothingItem HotShirt { get; private set; }
    public ClothingItem HotPants { get; private set; }

    [Header("Scene References")]
    public PlayerController playerController;
    public OutfitManager playerOutfitManager;
    public Sprite playerBodySprite;

    [Header("NPC Spawning")]
    public List<GameObject> npcPrefabs;
    public List<Transform> spawnPoints = new List<Transform>();

    private List<GameObject> spawnedNpcs = new List<GameObject>();
    private List<NPCController> activeNpcs = new List<NPCController>();


    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        // This is the new fail-safe logic
        if (PartyManager.instance == null)
        {
            Debug.LogWarning("PartyManager not found. A temporary PartyManager is being created for testing purposes. Please start from the TitleScene for a normal playthrough.");
            GameObject pm_instance = new GameObject("[DEBUG] PartyManager");
            pm_instance.AddComponent<PartyManager>();
            // Initialize the new PartyManager for a fresh game
            PartyManager.instance.StartNewGame();
        }

        PartyManager.instance.PrepareForNextRound();
        CurrentState = GameState.Investigation;

        HotHat = allHats[Random.Range(0, allHats.Count)];
        HotShirt = allShirts[Random.Range(0, allShirts.Count)];
        HotPants = allPants[Random.Range(0, allPants.Count)];

        SpawnNpcsForRound();

        foreach (var npc in activeNpcs)
        {
            npc.InitializeForRound();
        }
    }

    private void SpawnNpcsForRound()
    {
        foreach (var oldNpc in spawnedNpcs)
        {
            Destroy(oldNpc);
        }
        spawnedNpcs.Clear();
        activeNpcs.Clear();

        int[] guestCounts = { 3, 5, 7 };
        int partyIndex = PartyManager.instance.currentPartyNumber - 1;
        if (partyIndex < 0 || partyIndex >= guestCounts.Length) partyIndex = 0;
        int npcCount = guestCounts[partyIndex];

        if (npcCount > spawnPoints.Count)
        {
            Debug.LogError($"Not enough spawn points! Need {npcCount} but only {spawnPoints.Count} are available.");
            return;
        }

        var shuffledPrefabs = npcPrefabs.OrderBy(p => Random.value).ToList();
        var shuffledSpawnPoints = spawnPoints.OrderBy(p => Random.value).ToList();

        for (int i = 0; i < npcCount && i < shuffledPrefabs.Count; i++)
        {
            GameObject prefabToSpawn = shuffledPrefabs[i];
            Transform spawnPoint = shuffledSpawnPoints[i];

            GameObject newNpc = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            spawnedNpcs.Add(newNpc);
            activeNpcs.Add(newNpc.GetComponent<NPCController>());
        }
    }

    public void StartParty()
    {
        if (CurrentState != GameState.Investigation) return;
        CurrentState = GameState.PartyTime;
        playerController.ControlsLocked = true;

        foreach (var npc in activeNpcs) { npc.MakeFinalOutfitDecision(); }

        List<PartyGoerData> allPartyGoerData = new List<PartyGoerData>();
        allPartyGoerData.Add(new PartyGoerData { characterName = "Player", bodySprite = playerBodySprite, outfit = playerOutfitManager.currentOutfit });
        foreach (var npc in activeNpcs)
        {
            allPartyGoerData.Add(new PartyGoerData { characterName = npc.npcName, bodySprite = npc.portraitBodySprite, outfit = npc.GetFinalOutfit() });
        }

        PartyManager.instance.RegisterPartyGoers(allPartyGoerData);
        SceneManager.LoadScene("Party");
    }
}