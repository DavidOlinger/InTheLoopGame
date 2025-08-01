using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will be on every NPC. It manages their outfit and what happens
// when the player interacts with them.
public class NPCController : MonoBehaviour
{
    // PUBLIC VARIABLES
    // ------------------------------------------------------------------------------------

    [Header("NPC Info")]
    public string npcName = "Mysterious Stranger";

    [Header("Outfit Renderers")]
    public SpriteRenderer hatRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer pantsRenderer;

    [Header("Chosen Outfit")]
    public ClothingItem myHat;
    public ClothingItem myShirt;
    public ClothingItem myPants;


    //PRIVATE VARIABLES
    // ------------------------------------------------------------------------------------

    private bool isTalking = false;








    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------

    void Start()
    {
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
        // --- THIS IS THE ONLY PART THAT CHANGED! ---
        // Instead of Debug.Log, we now call our shiny new DialogueManager.
        // Because it's a singleton, we can access it from anywhere using ".instance".
        if (!isTalking)
        {
            string sentence = npcName + " says: 'I think " + myShirt.itemName + " is very in right now.'";
            DialogueManager.instance.StartDialogue(sentence);
        }
        else
        {
            DialogueManager.instance.EndDialogue();

        }
    }



    // PRIVATE HELPER METHODS
    // ------------------------------------------------------------------------------------

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
