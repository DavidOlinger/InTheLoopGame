// Located at: Assets/Scripts/GameLogicScripts/GameManager.cs
// FINAL VERSION WITH RANDOMIZED SPAWNING

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; // Added to easily shuffle the list of prefabs.

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState { Investigation, PartyTime }
    public GameState CurrentState { get; private set; }

    public List<ClothingItem> allHats;
    public List<ClothingItem> allShirts;
    public List<ClothingItem> allPants;

    public ClothingItem HotHat { get; private set; }
    public ClothingItem HotShirt { get; private set; }
    public ClothingItem HotPants { get; private set; }

    public PlayerController playerController;
    public OutfitManager playerOutfitManager;
    public Sprite playerBodySprite;

    // =========================================================================================
    // NEW: NPC SPAWNING AND SELECTION LOGIC
    [Header("NPC Spawning")]
    public List<GameObject> npcPrefabs; // Drag your 7 NPC prefabs here.
    public Rect spawnArea; // Define the rectangular area where NPCs can spawn.

    private List<GameObject> spawnedNpcs = new List<GameObject>(); // Tracks spawned NPCs for cleanup.
    private List<NPCController> activeNpcs = new List<NPCController>(); // Tracks active controllers.
    // =========================================================================================


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
        if (PartyManager.instance == null)
        {
            Debug.LogError("PartyManager not found! Please start from the TitleScreen.");
            return;
        }

        PartyManager.instance.PrepareForNextRound();
        CurrentState = GameState.Investigation;

        HotHat = allHats[Random.Range(0, allHats.Count)];
        HotShirt = allShirts[Random.Range(0, allShirts.Count)];
        HotPants = allPants[Random.Range(0, allPants.Count)];

        // This method now handles random selection and spawning.
        SpawnNpcsForRound();

        foreach (var npc in activeNpcs)
        {
            npc.InitializeForRound();
        }
    }

    // This method has been completely reworked.
    private void SpawnNpcsForRound()
    {
        // 1. Clean up NPCs from the previous round.
        foreach (var oldNpc in spawnedNpcs)
        {
            Destroy(oldNpc);
        }
        spawnedNpcs.Clear();
        activeNpcs.Clear();

        // 2. Determine how many NPCs to spawn.
        int[] guestCounts = { 3, 5, 7 };
        int partyIndex = PartyManager.instance.currentPartyNumber - 1;
        if (partyIndex < 0 || partyIndex >= guestCounts.Length) partyIndex = 0;
        int npcCount = guestCounts[partyIndex];

        // 3. Randomly select which NPCs to spawn from the prefab list.
        var shuffledPrefabs = npcPrefabs.OrderBy(p => Random.value).ToList();

        // 4. Instantiate the selected NPCs at random positions.
        for (int i = 0; i < npcCount && i < shuffledPrefabs.Count; i++)
        {
            GameObject prefabToSpawn = shuffledPrefabs[i];
            Vector2 spawnPosition = GetRandomSpawnPoint();

            GameObject newNpc = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnedNpcs.Add(newNpc); // Track for cleanup
            activeNpcs.Add(newNpc.GetComponent<NPCController>()); // Track for game logic
        }
        Debug.Log($"Party #{PartyManager.instance.currentPartyNumber}: Spawned {activeNpcs.Count} random NPCs.");
    }

    // NEW: Helper method to get a random point within the defined spawn area.
    private Vector2 GetRandomSpawnPoint()
    {
        float randomX = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width);
        float randomY = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height);
        return new Vector2(randomX, randomY);
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

    // NEW: Draws a helpful visual box in the Scene view to show the spawn area.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(spawnArea.x + spawnArea.width / 2, spawnArea.y + spawnArea.height / 2, 0), new Vector3(spawnArea.width, spawnArea.height, 0));
    }
}