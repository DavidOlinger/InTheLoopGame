using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This component can be attached to a UI parent object that holds all the Image layers for a character portrait.
public class PortraitUI : MonoBehaviour
{
    [Header("Image Components")]
    public Image bodyImage;
    public Image hatImage;
    public Image shirtImage;
    public Image pantsImage;

    // A single public method to update all the sprites based on a list of clothing items.
    public void DisplayOutfit(List<ClothingItem> outfit)
    {
        // Set default sprites or hide images before applying new ones
        hatImage.enabled = false;
        shirtImage.enabled = false;
        pantsImage.enabled = false;

        foreach (var item in outfit)
        {
            if (item == null) continue;

            switch (item.clothingType)
            {
                case ClothingType.Hat:
                    hatImage.sprite = item.itemSprite;
                    hatImage.enabled = true;
                    break;
                case ClothingType.Shirt:
                    shirtImage.sprite = item.itemSprite;
                    shirtImage.enabled = true;
                    break;
                case ClothingType.Pants:
                    pantsImage.sprite = item.itemSprite;
                    pantsImage.enabled = true;
                    break;
            }
        }
    }
}