using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will be attached to the Player. Its only job is to manage
// which clothes the player is currently wearing and update their sprites.
public class OutfitManager : MonoBehaviour
{
    // PUBLIC VARIABLES
    // We will drag the components from our child GameObjects into these slots in the Editor.
    // ------------------------------------------------------------------------------------

    [Header("Outfit Sprite Renderers")]
    // The SpriteRenderer component for the hat.
    public SpriteRenderer hatRenderer;
    // The SpriteRenderer component for the shirt.
    public SpriteRenderer shirtRenderer;
    // The SpriteRenderer component for the pants.
    public SpriteRenderer pantsRenderer;


    [Header("Starting Clothes")]
    // We can set the player's initial outfit directly from the editor.
    // Drag the ScriptableObject assets you created into these slots.
    public ClothingItem startingHat;
    public ClothingItem startingShirt;
    public ClothingItem startingPants;


    // PRIVATE VARIABLES
    // These will hold a reference to the *data* of the currently equipped items.
    // ------------------------------------------------------------------------------------
    private ClothingItem currentHat;
    private ClothingItem currentShirt;
    private ClothingItem currentPants;


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------

    // Start() is called once before the first frame update.
    // It's the perfect place to set up the initial state of the character.
    void Start()
    {
        // Equip the starting clothes we assigned in the editor.
        EquipItem(startingHat);
        EquipItem(startingShirt);
        EquipItem(startingPants);
    }


    // CUSTOM PUBLIC METHODS
    // These are functions we can call from other scripts later on.
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Equips a new clothing item, updating the correct sprite and data reference.
    /// </summary>
    /// <param name="itemToEquip">The ScriptableObject data for the item to wear.</param>
    public void EquipItem(ClothingItem itemToEquip)
    {
        // First, check if the item is null. If so, do nothing.
        if (itemToEquip == null)
        {
            Debug.LogWarning("Tried to equip a null item.");
            return;
        }

        // Use a switch statement to check the item's ClothingType.
        // This is cleaner than a bunch of if-else statements.
        switch (itemToEquip.clothingType)
        {
            case ClothingType.Hat:
                currentHat = itemToEquip;
                hatRenderer.sprite = itemToEquip.itemSprite;
                break;

            case ClothingType.Shirt:
                currentShirt = itemToEquip;
                shirtRenderer.sprite = itemToEquip.itemSprite;
                break;

            case ClothingType.Pants:
                currentPants = itemToEquip;
                pantsRenderer.sprite = itemToEquip.itemSprite;
                break;
        }
    }

    // NOTE: In the future, you could add a function here like:
    // public ClothingItem GetCurrentItem(ClothingType type)
    // This would be useful for NPCs to check what the player is wearing.
}
