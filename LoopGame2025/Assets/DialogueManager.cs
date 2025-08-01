using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Make sure to include this line to use TextMeshPro!

// This script will manage the entire dialogue UI. We will make it a "Singleton",
// which means there will only ever be one instance of it in the entire game.
public class DialogueManager : MonoBehaviour
{
    // PUBLIC VARIABLES
    // ------------------------------------------------------------------------------------

    [Header("UI Components")]
    // A reference to the parent panel of our dialogue box.
    public GameObject dialoguePanel;
    // A reference to the TextMeshPro component where the text will be displayed.
    public TextMeshProUGUI dialogueText;


    // PRIVATE & STATIC VARIABLES
    // ------------------------------------------------------------------------------------

    // This is the core of the Singleton pattern. A static variable is shared
    // across all instances of a class. Since we will only have one, it becomes
    // a global access point.
    public static DialogueManager instance;


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------

    // Awake() is called before Start(). It's the perfect place to set up a singleton.
    private void Awake()
    {
        // --- Singleton Setup ---
        // If an instance of this already exists and it's not this one, destroy this one.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Otherwise, set the instance to this. This is the one and only.
            instance = this;
        }
    }

    void Start()
    {
        // Make sure the dialogue box is hidden when the game starts.
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // This allows the player to close the dialogue box.
        // We check if the panel is active and if the interact key is pressed.
        if (dialoguePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
        {
            EndDialogue();
        }
    }


    // CUSTOM PUBLIC METHODS
    // ------------------------------------------------------------------------------------

    /// <summary>
    /// Call this from any other script to start a dialogue.
    /// </summary>
    /// <param name="sentence">The line of dialogue to display.</param>
    public void StartDialogue(string sentence)
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            // Turn on the dialogue panel.
            dialoguePanel.SetActive(true);
            // Set the text to the sentence we were given.
            dialogueText.text = sentence;
        }

    }

    /// <summary>
    /// Hides the dialogue panel.
    /// </summary>
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
