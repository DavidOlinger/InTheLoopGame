using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC VARIABLES
    // ------------------------------------------------------------------------------------
    public float moveSpeed = 5f;
    public bool IsInDialogue { get; private set; }


    // PRIVATE VARIABLES
    // ------------------------------------------------------------------------------------
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private NPCController currentInteractableNPC;
    private DialogueNode currentNode;
    private int selectedOption = 0; // 0 for left (A), 1 for right (D)


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IsInDialogue = false;
    }

    void Update()
    {
        // --- MOVEMENT LOGIC ---
        // Player can only move if not in dialogue.
        if (!IsInDialogue)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        // --- INTERACTION & DIALOGUE LOGIC ---
        HandleDialogueInput();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }


    // --- DIALOGUE HANDLING ---
    // ------------------------------------------------------------------------------------
    private void HandleDialogueInput()
    {
        // Handle starting dialogue or skipping the typewriter with the E key.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If we're not in dialogue but are near an NPC, start the conversation.
            if (!IsInDialogue && currentInteractableNPC != null)
            {
                StartConversation();
                return; // Exit early to avoid other checks this frame.
            }

            // If we are in dialogue, the 'E' key's function depends on the manager's state.
            if (IsInDialogue)
            {
                switch (DialogueManager.instance.CurrentState)
                {
                    case DialogueManager.DialogueState.Typing:
                        // If text is typing, E skips it.
                        DialogueManager.instance.SkipTyping();
                        break;

                    // MODIFIED: 'E' now only advances if there are NO options.
                    case DialogueManager.DialogueState.Displaying:
                        if (currentNode.options.Count == 0)
                        {
                            AdvanceConversation();
                        }
                        break;
                }
            }
        }

        // MODIFIED: Handle option selection and advancement with A and D.
        if (IsInDialogue && DialogueManager.instance.CurrentState == DialogueManager.DialogueState.Displaying && currentNode.options.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedOption = 0;
                AdvanceConversation(); // Advance immediately after selecting.
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                selectedOption = 1;
                AdvanceConversation(); // Advance immediately after selecting.
            }
        }
    }

    private void StartConversation()
    {
        IsInDialogue = true;
        currentNode = currentInteractableNPC.startingDialogue;
        DialogueManager.instance.StartDialogue(currentNode);
    }

    private void AdvanceConversation()
    {
        // Check if the current node has options.
        if (currentNode != null && currentNode.options.Count > 0)
        {
            // Make sure the selected option is valid.
            if (selectedOption < currentNode.options.Count)
            {
                DialogueNode nextNode = currentNode.options[selectedOption].nextNode;
                if (nextNode != null)
                {
                    currentNode = nextNode;
                    DialogueManager.instance.StartDialogue(currentNode);
                }
                else
                {
                    EndConversation();
                }
            }
        }
        else
        {
            // If there were no options, just end the conversation.
            EndConversation();
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
    // ------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentInteractableNPC = other.GetComponent<NPCController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
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