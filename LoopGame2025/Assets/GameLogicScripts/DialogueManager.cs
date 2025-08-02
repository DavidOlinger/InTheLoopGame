using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This manager now includes a state machine to handle different phases of dialogue.
public class DialogueManager : MonoBehaviour
{
    // This enum defines the possible states our dialogue can be in.
    public enum DialogueState { Idle, Typing, Displaying }

    // PUBLIC VARIABLES
    // ------------------------------------------------------------------------------------
    [Header("UI Components")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI optionsText;

    [Header("Typing Effect")]
    public float typingSpeed = 0.04f;

    // This public property lets other scripts (like the PlayerController) know our current state.
    public DialogueState CurrentState { get; private set; }


    // PRIVATE & STATIC VARIABLES
    // ------------------------------------------------------------------------------------
    public static DialogueManager instance;
    private Coroutine typingCoroutine;
    private string fullSentence; // Store the full sentence to allow skipping.


    // UNITY LIFECYCLE METHODS
    // ------------------------------------------------------------------------------------
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        CurrentState = DialogueState.Idle; // Start in the Idle state.
    }

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }


    // CUSTOM PUBLIC METHODS
    // ------------------------------------------------------------------------------------

    public void StartDialogue(DialogueNode node)
    {
        if (node == null) return;

        dialoguePanel.SetActive(true);

        // Stop any previous coroutine to prevent weird overlaps.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(node));

        DisplayOptions(node);
    }

    public void EndDialogue()
    {
        // Stop any running coroutines when ending dialogue.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        CurrentState = DialogueState.Idle;
        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// This new function handles skipping the typewriter effect.
    /// </summary>
    public void SkipTyping()
    {
        // If we are not typing, do nothing.
        if (CurrentState != DialogueState.Typing) return;

        // Stop the typing coroutine, display the full text instantly, and change state.
        StopCoroutine(typingCoroutine);
        dialogueText.text = fullSentence;
        CurrentState = DialogueState.Displaying;
    }


    // PRIVATE & HELPER METHODS
    // ------------------------------------------------------------------------------------

    private IEnumerator TypeSentence(DialogueNode node)
    {
        // Set the state to Typing.
        CurrentState = DialogueState.Typing;
        dialogueText.text = "";
        fullSentence = node.sentence; // Store the full sentence.

        foreach (char letter in fullSentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Once typing is finished, change the state to Displaying.
        CurrentState = DialogueState.Displaying;
    }

    private void DisplayOptions(DialogueNode node)
    {
        optionsText.text = "";
        if (node.options != null && node.options.Count > 0)
        {
            // This logic is unchanged, but now it's in its own clean function.
            string optionsString = $"A: {node.options[0].responseText}    D: {node.options[1].responseText}";
            optionsText.text = optionsString;
            optionsText.gameObject.SetActive(true);
        }
        else
        {
            optionsText.gameObject.SetActive(false);
        }
    }
}
