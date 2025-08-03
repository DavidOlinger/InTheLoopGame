using System.Collections.Generic;
using UnityEngine;

public class OutfitManager : MonoBehaviour
{
    [Header("Available Clothing Lists")]
    public List<ClothingItem> availableHats = new List<ClothingItem>();
    public List<ClothingItem> availableShirts = new List<ClothingItem>();
    public List<ClothingItem> availablePants = new List<ClothingItem>();

    // MODIFIED: This dictionary now holds the player's currently equipped items.
    public Dictionary<ClothingType, ClothingItem> currentOutfit { get; private set; }

    private int currentHatIndex = -1;
    private int currentShirtIndex = -1;
    private int currentPantsIndex = -1;

    void Awake()
    {
        currentOutfit = new Dictionary<ClothingType, ClothingItem>();
        // Initialize with the first item from each list, if available
        CycleClothing(ClothingType.Hat);
        CycleClothing(ClothingType.Shirt);
        CycleClothing(ClothingType.Pants);
    }

    // The Update method is removed for now. We will call CycleClothing from a UI menu later.

    public void CycleClothing(ClothingType type)
    {
        switch (type)
        {
            case ClothingType.Hat:
                if (availableHats.Count == 0) return;
                currentHatIndex = (currentHatIndex + 1) % availableHats.Count;
                currentOutfit[ClothingType.Hat] = availableHats[currentHatIndex];
                break;
            case ClothingType.Shirt:
                if (availableShirts.Count == 0) return;
                currentShirtIndex = (currentShirtIndex + 1) % availableShirts.Count;
                currentOutfit[ClothingType.Shirt] = availableShirts[currentShirtIndex];
                break;
            case ClothingType.Pants:
                if (availablePants.Count == 0) return;
                currentPantsIndex = (currentPantsIndex + 1) % availablePants.Count;
                currentOutfit[ClothingType.Pants] = availablePants[currentPantsIndex];
                break;
        }
    }
}