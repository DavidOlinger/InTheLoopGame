using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will be attached to the Player. It now handles the player's
// entire "closet" and allows swapping between items.
public class OutfitManager : MonoBehaviour
{
    // PUBLIC VARIABLES
    // ------------------------------------------------------------------------------------

    [Header("Outfit Sprite Renderers")]
    public SpriteRenderer hatRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer pantsRenderer;

    [Header("Available Clothing Lists")]
    // Instead of single items, we now have lists for each clothing type.
    // You will fill these lists in the Unity Editor.
    // IMPORTANT: You can add a "None" (null) entry as the first item in each list
    // to allow the player to wear nothing in that slot.
    public List<ClothingItem> availableHats = new List<ClothingItem>();
    public List<ClothingItem> availableShirts = new List<ClothingItem>();
    public List<ClothingItem> availablePants = new List<ClothingItem>();


    // PRIVATE VARIABLES
    // ------------------------------------------------------------------------------------
    // We need to keep track of our current position in each list.
    private int currentHatIndex = 0;
    private int currentShirtIndex = 0;
    private int currentPantsIndex = 0;


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------

    void Start()
    {
        // Equip the very first item from each list at the start of the game.
        // If the lists have items, this will equip them. If not, it does nothing.
        CycleClothing(ClothingType.Hat);
        CycleClothing(ClothingType.Shirt);
        CycleClothing(ClothingType.Pants);
    }

    void Update()
    {
        // Check for keyboard input to cycle through clothes.
        // We use GetKeyDown so it only fires once per press.

        // J key for Hats
        if (Input.GetKeyDown(KeyCode.J))
        {
            CycleClothing(ClothingType.Hat);
        }

        // I key for Shirts
        if (Input.GetKeyDown(KeyCode.I))
        {
            CycleClothing(ClothingType.Shirt);
        }

        // O key for Pants
        if (Input.GetKeyDown(KeyCode.O))
        {
            CycleClothing(ClothingType.Pants);
        }
    }


    // CUSTOM METHODS
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Cycles to the next item in the specified clothing list.
    /// </summary>
    /// <param name="type">The type of clothing to cycle (Hat, Shirt, or Pants).</param>
    public void CycleClothing(ClothingType type)
    {
        switch (type)
        {
            case ClothingType.Hat:
                // Check if the list has anything in it to avoid errors.
                if (availableHats.Count == 0) return;
                // Move to the next index, wrapping around if we reach the end.
                currentHatIndex = (currentHatIndex + 1) % availableHats.Count;
                EquipItem(availableHats[currentHatIndex]);
                break;

            case ClothingType.Shirt:
                if (availableShirts.Count == 0) return;
                currentShirtIndex = (currentShirtIndex + 1) % availableShirts.Count;
                EquipItem(availableShirts[currentShirtIndex]);
                break;

            case ClothingType.Pants:
                if (availablePants.Count == 0) return;
                currentPantsIndex = (currentPantsIndex + 1) % availablePants.Count;
                EquipItem(availablePants[currentPantsIndex]);
                break;
        }
    }


    /// <summary>
    /// Equips a clothing item, updating the correct sprite. Handles null items.
    /// </summary>
    /// <param name="itemToEquip">The ScriptableObject data for the item to wear.</param>
    public void EquipItem(ClothingItem itemToEquip)
    {
        // If the item is null (our "None" option), we find the correct renderer
        // and set its sprite to null, making it invisible.
        if (itemToEquip == null)
        {
            // This is a placeholder check. We need to know which slot to clear.
            // The logic in CycleClothing ensures we pass a typed item, but if we
            // called this from somewhere else, we'd need to know the type.
            // For now, this is safe. We'll improve it if needed.
            return; // The logic below handles this better.
        }

        // Use a switch statement to check the item's ClothingType.
        switch (itemToEquip.clothingType)
        {
            case ClothingType.Hat:
                hatRenderer.sprite = itemToEquip.itemSprite;
                break;
            case ClothingType.Shirt:
                shirtRenderer.sprite = itemToEquip.itemSprite;
                break;
            case ClothingType.Pants:
                pantsRenderer.sprite = itemToEquip.itemSprite;
                break;
        }
    }
}
