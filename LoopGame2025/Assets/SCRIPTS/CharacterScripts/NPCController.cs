// Located at: Assets/Scripts/CharacterScripts/NPCController.cs
// CORRECTED AND VERIFIED VERSION

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCController : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Mysterious Stranger";
    public DialogueNode startingDialogue;
    public Sprite portraitBodySprite;

    [Header("Default Style")]
    public ClothingItem defaultHat;
    public ClothingItem defaultShirt;
    public ClothingItem defaultPants;

    [Header("Behavior")]
    [Range(0f, 1f)]
    public float trendsetterChance = 0.25f;

    private Dictionary<ClothingType, ClothingItem> initialOutfit = new Dictionary<ClothingType, ClothingItem>();
    private Dictionary<ClothingType, ClothingItem> finalOutfit = new Dictionary<ClothingType, ClothingItem>();
    private Dictionary<ClothingItem, float> opinionScores = new Dictionary<ClothingItem, float>();

    // This method is called ONLY by the GameManager after it has defined the hot items.
    public void InitializeForRound()
    {
        initialOutfit.Clear();
        finalOutfit.Clear();
        opinionScores.Clear();

        // This line will now work because GameManager has already assigned a value to HotHat.
        initialOutfit[ClothingType.Hat] = Random.value < trendsetterChance ? GameManager.instance.HotHat : defaultHat;
        initialOutfit[ClothingType.Shirt] = Random.value < trendsetterChance ? GameManager.instance.HotShirt : defaultShirt;
        initialOutfit[ClothingType.Pants] = Random.value < trendsetterChance ? GameManager.instance.HotPants : defaultPants;
    }

    public void BeInfluenced(ClothingItem suggestedItem, float influenceAmount)
    {
        if (suggestedItem == null) return;
        if (!opinionScores.ContainsKey(suggestedItem))
        {
            opinionScores[suggestedItem] = 0;
        }
        opinionScores[suggestedItem] += influenceAmount;
    }

    public void MakeFinalOutfitDecision()
    {
        finalOutfit.Clear();
        finalOutfit[ClothingType.Hat] = DecideOnItem(ClothingType.Hat);
        finalOutfit[ClothingType.Shirt] = DecideOnItem(ClothingType.Shirt);
        finalOutfit[ClothingType.Pants] = DecideOnItem(ClothingType.Pants);
    }

    private ClothingItem DecideOnItem(ClothingType type)
    {
        var relevantItems = opinionScores.Keys
            .Where(item => item != null && item.clothingType == type)
            .ToList();

        if (relevantItems.Count == 0)
        {
            return initialOutfit[type];
        }

        ClothingItem mostPopularItem = relevantItems.OrderByDescending(item => opinionScores[item]).First();

        if (opinionScores[mostPopularItem] > 0)
        {
            return mostPopularItem;
        }

        return initialOutfit[type];
    }

    public Dictionary<ClothingType, ClothingItem> GetFinalOutfit()
    {
        return finalOutfit;
    }

    public List<ClothingItem> GetCurrentOutfit()
    {
        return new List<ClothingItem>(initialOutfit.Values);
    }

    public string GetFashionGossip()
    {
        if (GameManager.instance.HotHat == null || GameManager.instance.HotShirt == null || GameManager.instance.HotPants == null)
        {
            return "I'm drawing a blank on the latest trends...";
        }
        int rand = Random.Range(0, 3);
        if (rand == 0) return $"I've heard <color=yellow>{GameManager.instance.HotHat.itemName}</color> are very in right now.";
        if (rand == 1) return $"Everyone is talking about <color=yellow>{GameManager.instance.HotShirt.itemName}</color>.";
        return $"You simply must get a pair of <color=yellow>{GameManager.instance.HotPants.itemName}</color>.";
    }
}