using System.Collections.Generic;
using UnityEngine;

public class OutfitMenuUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject outfitMenuPanel;
    public GameObject playerPortraitObject; // Reference to the shared portrait object

    [Header("Data References")]
    public OutfitManager playerOutfitManager;
    public PlayerController playerController;
    public Sprite playerBodySprite;

    private bool isMenuOpen = false;
    private PortraitUI playerPortraitUI;

    void Start()
    {
        playerPortraitUI = playerPortraitObject.GetComponent<PortraitUI>();
        outfitMenuPanel.SetActive(false);
        playerPortraitObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMenuOpen = !isMenuOpen;
            ToggleMenu();
        }

        if (isMenuOpen)
        {
            HandleClothingInput();
        }
    }

    private void ToggleMenu()
    {
        outfitMenuPanel.SetActive(isMenuOpen);
        playerPortraitObject.SetActive(isMenuOpen); // Toggle the shared portrait
        playerController.IsMenuOpen = isMenuOpen;

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

    private void UpdateOutfitDisplay()
    {
        playerPortraitUI.DisplayOutfit(playerBodySprite, new List<ClothingItem>(playerOutfitManager.currentOutfit.Values));
    }
}