// Located at: Assets/Scripts/UIScripts/PortraitUI.cs
// FINAL VERSION FOR PHASE 3, STEP 2.B

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added to support TextMeshPro

public class PortraitUI : MonoBehaviour
{
    [Header("Image Components")]
    public Image bodyImage;
    public Image hatImage;
    public Image shirtImage;
    public Image pantsImage;

    [Header("Optional Name Label")]
    public TextMeshProUGUI nameText; // NEW: A reference for an optional name label.

    // MODIFIED: Added an optional 'characterName' parameter.
    public void DisplayOutfit(Sprite bodySprite, List<ClothingItem> outfit, string characterName = "")
    {
        // Set the body sprite first.
        bodyImage.sprite = bodySprite;
        bodyImage.enabled = (bodySprite != null);

        // Update the name text if the reference exists.
        if (nameText != null)
        {
            nameText.text = characterName;
            nameText.enabled = !string.IsNullOrEmpty(characterName);
        }

        // Disable all clothing layers by default before applying the new outfit.
        hatImage.enabled = false;
        shirtImage.enabled = false;
        pantsImage.enabled = false;

        if (outfit == null) return;

        // Iterate through the provided outfit and enable/set the sprite for each item.
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