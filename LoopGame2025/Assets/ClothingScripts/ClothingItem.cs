using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an ENUM. It's a simple way to create a list of options that we can
// pick from in the editor. This ensures we can't make typos.
public enum ClothingType { Hat, Shirt, Pants }


// This is the magic line for ScriptableObjects. It tells Unity to add a new option
// to the "Create" menu, so we can easily make new clothing items as assets.
[CreateAssetMenu(fileName = "New Clothing Item", menuName = "Fashion/New Clothing Item")]
public class ClothingItem : ScriptableObject
{
    // PUBLIC VARIABLES
    // These are the "stats" or data for each piece of clothing.
    // Because this is a ScriptableObject, we'll fill this out in the Project window,
    // not on a GameObject in the scene.
    // ------------------------------------------------------------------------------------

    // The name of the item, e.g., "Top Hat" or "Cargo Shorts".
    public string itemName;

    // The type of clothing this is. We use the enum we created above.
    public ClothingType clothingType;

    // The actual image that will be displayed for this item.
    // We will drag a Sprite asset into this slot in the Inspector.
    public Sprite itemSprite;

    // FUTURE IDEAS:
    // You could add more data here later! For example:
    // public string itemDescription;
    // public int fashionScore;
    // public Faction associatedFaction;
}
