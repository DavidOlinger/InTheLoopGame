using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will be on every NPC. It manages their outfit and what happens
// when the player interacts with them.
public class NPCController : MonoBehaviour
{
    // PUBLIC VARIABLES
    // We can set these up in the editor for each individual NPC.
    // ------------------------------------------------------------------------------------

    [Header("NPC Info")]
    // Give each NPC a unique name for debugging and dialogue.
    public string npcName = "Mysterious Stranger";

    [Header("Outfit Renderers")]
    // The SpriteRenderers for this NPC's clothes.
    public SpriteRenderer hatRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer pantsRenderer;

    [Header("Chosen Outfit")]
    // The actual clothing items this NPC has decided to wear.
    // Drag the ScriptableObject assets here in the Inspector.
    public ClothingItem myHat;
    public ClothingItem myShirt;
    public ClothingItem myPants;


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------

    void Start()
    {
        // At the start of the game, make the NPC wear their chosen clothes.
        // This uses the same logic as the player's OutfitManager.
        EquipItem(myHat);
        EquipItem(myShirt);
        EquipItem(myPants);
    }


    // CUSTOM PUBLIC METHODS
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// This function is called by the PlayerController when the player interacts.
    /// </summary>
    public void OnInteract()
    {
        // For now, we'll just print a message to the console to prove it works.
        // This is where you'll eventually trigger the dialogue UI.
        Debug.Log(npcName + " says: 'I think " + myShirt.itemName + " is very in right now.'");

        // FUTURE:
        // Instead of Debug.Log, you would call your DialogueManager here.
        // Example: DialogueManager.instance.StartDialogue(npcName, "My dialogue sentence.");
    }


    // PRIVATE HELPER METHODS
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// A helper function to update the NPC's sprites based on their clothing data.
    /// </summary>
    private void EquipItem(ClothingItem itemToEquip)
    {
        if (itemToEquip == null) return;

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
