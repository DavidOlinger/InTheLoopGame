using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Image

public class OutfitMenuUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject outfitMenuPanel;
    public Image playerBodyImage;
    public Image hatImage;
    public Image shirtImage;
    public Image pantsImage;

    [Header("References")]
    public OutfitManager playerOutfitManager;
    public PlayerController playerController;

    private bool isMenuOpen = false;

    void Start()
    {
        // Ensure the menu is hidden at the start
        outfitMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Toggle the menu with the Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuOpen = !isMenuOpen;
            ToggleMenu();
        }

        // Only listen for clothing changes if the menu is open
        if (isMenuOpen)
        {
            HandleClothingInput();
        }
    }

    private void ToggleMenu()
    {
        outfitMenuPanel.SetActive(isMenuOpen);
        playerController.IsMenuOpen = isMenuOpen; // Pause/unpause player movement

        // When opening the menu, update the display to show the current outfit
        if (isMenuOpen)
        {
            UpdateOutfitDisplay();
        }
    }

    private void HandleClothingInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerOutfitManager.CycleClothing(ClothingType.Hat);
            UpdateOutfitDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            playerOutfitManager.CycleClothing(ClothingType.Shirt);
            UpdateOutfitDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            playerOutfitManager.CycleClothing(ClothingType.Pants);
            UpdateOutfitDisplay();
        }
    }

    // This method reads the data from OutfitManager and updates the UI Images
    private void UpdateOutfitDisplay()
    {
        // Update Hat
        if (playerOutfitManager.currentOutfit.TryGetValue(ClothingType.Hat, out ClothingItem hatItem))
        {
            hatImage.sprite = hatItem.itemSprite;
            hatImage.enabled = true;
        }
        else
        {
            hatImage.enabled = false;
        }

        // Update Shirt
        if (playerOutfitManager.currentOutfit.TryGetValue(ClothingType.Shirt, out ClothingItem shirtItem))
        {
            shirtImage.sprite = shirtItem.itemSprite;
            shirtImage.enabled = true;
        }
        else
        {
            shirtImage.enabled = false;
        }

        // Update Pants
        if (playerOutfitManager.currentOutfit.TryGetValue(ClothingType.Pants, out ClothingItem pantsItem))
        {
            pantsImage.sprite = pantsItem.itemSprite;
            pantsImage.enabled = true;
        }
        else
        {
            pantsImage.enabled = false;
        }
    }
}