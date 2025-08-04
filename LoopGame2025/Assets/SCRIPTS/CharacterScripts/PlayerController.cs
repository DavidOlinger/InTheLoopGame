// Located at: Assets/Scripts/CharacterScripts/PlayerController.cs
// FINAL VERSION FOR PHASE 3, STEP 1

using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // --- PUBLIC VARIABLES ---
    [Header("Configuration")]
    public float moveSpeed = 5f; // Speed of the player character.

    // --- PUBLIC PROPERTIES ---
    public bool IsInDialogue { get; private set; }
    public bool IsMenuOpen { get; set; }
    public bool ControlsLocked { get; set; } // NEW: Used by GameManager to lock input.

    // --- PRIVATE VARIABLES ---
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private NPCController currentInteractableNPC;
    private DialogueNode currentNode;
    private int selectedOption = 0;

    // --- UNITY LIFECYCLE METHODS ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        IsInDialogue = false;
        ControlsLocked = false; // Ensure controls are unlocked at start.
    }

    void Update()
    {
        // No input is processed if controls are locked.
        if (ControlsLocked)
        {
            // Ensure velocity is zeroed out if controls get locked while moving.
            if (rb.velocity != Vector2.zero)
            {
                rb.velocity = Vector2.zero;
                animator.SetFloat("isMoving", 0);
            }
            return;
        }

        HandleMovement();
        HandleDialogueInput();
    }

    void FixedUpdate()
    {
        // Physics update only happens if controls aren't locked.
        if (ControlsLocked) return;
        rb.velocity = moveInput * moveSpeed;
    }

    // --- MOVEMENT & ANIMATION ---
    private void HandleMovement()
    {
        if (!IsInDialogue && !IsMenuOpen)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        animator.SetFloat("isMoving", moveInput.sqrMagnitude);
        if (moveInput.sqrMagnitude > 0.1)
        {
            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);
        }
    }

    // --- DIALOGUE HANDLING ---
    private void HandleDialogueInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!IsInDialogue && currentInteractableNPC != null)
            {
                StartConversation();
                return;
            }

            if (IsInDialogue)
            {
                var dialogueManager = DialogueManager.instance;
                if (dialogueManager.CurrentState == DialogueManager.DialogueState.Typing)
                {
                    dialogueManager.SkipTyping();
                }
                else if (dialogueManager.CurrentState == DialogueManager.DialogueState.Displaying && (currentNode == null || currentNode.options.Count == 0))
                {
                    EndConversation();
                }
            }
        }
    }

    public void OnOptionSelected(int optionIndex)
    {
        if (!IsInDialogue) return;

        DialogueOption selectedOptionData = currentNode.options[optionIndex];

        switch (selectedOptionData.optionType)
        {
            case DialogueOption.OptionType.AskClothes:
                string gossip = currentInteractableNPC.GetFashionGossip();
                Debug.Log($"Gossip from {currentInteractableNPC.npcName}: {gossip}");
                break;

            case DialogueOption.OptionType.TellClothes:
                List<ClothingItem> playerOutfit = new List<ClothingItem>(GetComponent<OutfitManager>().currentOutfit.Values);
                if (playerOutfit.Count > 0)
                {
                    ClothingItem itemToSuggest = playerOutfit[Random.Range(0, playerOutfit.Count)];
                    if (itemToSuggest != null)
                    {
                        currentInteractableNPC.BeInfluenced(itemToSuggest, 1.0f);
                    }
                }
                break;

            default:
                selectedOption = optionIndex;
                AdvanceConversation();
                break;
        }
    }

    private void StartConversation()
    {
        IsInDialogue = true;
        currentNode = currentInteractableNPC.startingDialogue;
        DialogueManager.instance.StartDialogue(currentNode, currentInteractableNPC);
    }

    private void AdvanceConversation()
    {
        if (currentNode != null && currentNode.options.Count > 0)
        {
            if (selectedOption < currentNode.options.Count)
            {
                DialogueNode nextNode = currentNode.options[selectedOption].nextNode;
                if (nextNode != null)
                {
                    currentNode = nextNode;
                    DialogueManager.instance.ShowNode(nextNode);
                }
                else
                {
                    EndConversation();
                }
            }
        }
    }

    private void EndConversation()
    {
        IsInDialogue = false;
        DialogueManager.instance.EndDialogue();
        currentNode = null;
        selectedOption = 0;
    }

    // --- TRIGGER DETECTION ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ControlsLocked) return;
        if (other.CompareTag("NPC"))
        {
            currentInteractableNPC = other.GetComponent<NPCController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (ControlsLocked) return;
        if (other.CompareTag("NPC") && other.GetComponent<NPCController>() == currentInteractableNPC)
        {
            currentInteractableNPC = null;
            if (IsInDialogue)
            {
                EndConversation();
            }
        }
    }
}