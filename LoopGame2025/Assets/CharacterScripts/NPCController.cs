using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Mysterious Stranger";
    public DialogueNode startingDialogue;

    // MODIFIED: This now holds the NPC's outfit data, not the sprites.
    [Header("Chosen Outfit Data")]
    public ClothingItem myHat;
    public ClothingItem myShirt;
    public ClothingItem myPants;

    // We will use this later to get the full outfit for the portrait UI
    public List<ClothingItem> GetCurrentOutfit()
    {
        return new List<ClothingItem> { myHat, myShirt, myPants };
    }
}