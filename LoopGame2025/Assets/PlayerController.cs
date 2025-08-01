using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will handle all player movement and input.
// We are now adding interaction logic.
public class PlayerController : MonoBehaviour
{
    // PUBLIC VARIABLES
    // ------------------------------------------------------------------------------------
    public float moveSpeed = 5f;


    // PRIVATE VARIABLES
    // ------------------------------------------------------------------------------------
    private Vector2 moveInput;
    private Rigidbody2D rb; // A reference to the Rigidbody2D component for physics-based movement.

    // This will store a reference to the NPC we are currently able to talk to.
    // It will be null if we are not in range of any NPC.
    private NPCController currentInteractableNPC;


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------

    // Start is called before the first frame update. Good for grabbing components.
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject.
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // --- Input Reading ---
        // It's best practice to read input in Update().
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();


        // --- Interaction Input (NEW!) ---
        // Check if the "Interact" key is pressed (E key by default).
        if (Input.GetKeyDown(KeyCode.E))
        {
            // We also check if we are actually in range of an NPC.
            if (currentInteractableNPC != null)
            {
                // If we are, call their interaction function!
                currentInteractableNPC.OnInteract();
            }
        }
    }

    // FixedUpdate is called on a fixed time interval. It's the best place for physics calculations.
    void FixedUpdate()
    {
        // --- Movement Application ---
        // We apply the movement to the Rigidbody's velocity.
        // This results in smooth, physics-based movement.
        rb.velocity = moveInput * moveSpeed;
    }


    // --- TRIGGER DETECTION METHODS (NEW!) ---
    // These functions are called automatically by Unity's physics engine
    // when our player's collider enters or exits another collider that is a "Trigger".

    /// <summary>
    /// Called when this GameObject enters a trigger collider.
    /// </summary>
    /// <param name="other">The collider of the object we ran into.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // We check if the object we collided with has the "NPC" tag.
        if (other.CompareTag("NPC"))
        {
            // If it does, we get its NPCController component and store it.
            // This means we are now "in range" of this NPC.
            currentInteractableNPC = other.GetComponent<NPCController>();
            Debug.Log("Entered range of: " + currentInteractableNPC.npcName);
        }
    }

    /// <summary>
    /// Called when this GameObject exits a trigger collider.
    /// </summary>
    /// <param name="other">The collider of the object we just left.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // We check if the object we are leaving is the same one we are currently targeting.
        // This prevents bugs if we walk through multiple NPC triggers at once.
        if (other.CompareTag("NPC") && other.GetComponent<NPCController>() == currentInteractableNPC)
        {
            // If it is, we clear our reference. We can no longer interact with this NPC.
            Debug.Log("Left range of: " + currentInteractableNPC.npcName);
            currentInteractableNPC = null;
        }
    }
}
