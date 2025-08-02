using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Mysterious Stranger";
    public DialogueNode startingDialogue;

    [Header("Outfit Renderers")]
    public SpriteRenderer hatRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer pantsRenderer;

    [Header("Chosen Outfit")]
    public ClothingItem myHat;
    public ClothingItem myShirt;
    public ClothingItem myPants;

    void Start()
    {
        EquipItem(myHat);
        EquipItem(myShirt);
        EquipItem(myPants);
    }

    public void OnInteract()
    {
        DialogueManager.instance.StartDialogue(startingDialogue);
    }

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
