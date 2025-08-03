using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PUBLIC VARIABLES
    public float moveSpeed = 5f;
    public bool IsInDialogue { get; private set; }
    public bool IsMenuOpen { get; set; }

    // PRIVATE VARIABLES
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private NPCController currentInteractableNPC;
    private DialogueNode currentNode;
    private int selectedOption = 0;

    // ADDED: Animator reference
    private Animator animator;

    // UNITY LIFECYCLE METHODS
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // ADDED: Get the Animator component
        animator = GetComponent<Animator>();
        IsInDialogue = false;
    }

    void Update()
    {
        // MODIFIED: Player can only move if not in dialogue AND menu is not open.
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

        // ADDED: Update animator parameters
        animator.SetFloat("isMoving", moveInput.sqrMagnitude);
        if (moveInput.sqrMagnitude > 0.1) // Only update facing direction when moving
        {
            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);
        }

        HandleDialogueInput();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    // --- DIALOGUE HANDLING (No changes here) ---
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
                switch (DialogueManager.instance.CurrentState)
                {
                    case DialogueManager.DialogueState.Typing:
                        DialogueManager.instance.SkipTyping();
                        break;
                    case DialogueManager.DialogueState.Displaying:
                        if (currentNode.options.Count == 0)
                        {
                            AdvanceConversation();
                        }
                        break;
                }
            }
        }

    }


    public void OnOptionSelected(int optionIndex)
    {
        if (!IsInDialogue) return;

        selectedOption = optionIndex;
        AdvanceConversation();
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
                    DialogueManager.instance.StartDialogue(currentNode, currentInteractableNPC);
                }
                else
                {
                    EndConversation();
                }
            }
        }
        else
        {
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