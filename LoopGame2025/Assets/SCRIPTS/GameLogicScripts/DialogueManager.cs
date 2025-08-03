using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for Button

public class DialogueManager : MonoBehaviour
{
    public enum DialogueState { Idle, Typing, Displaying }
    public DialogueState CurrentState { get; private set; }
    public static DialogueManager instance;

    [Header("New UI Components")]
    public GameObject dialogueUIParent;
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons; // Assign your 4 buttons here

    [Header("Portrait References")]
    public PortraitUI playerPortrait;
    public PortraitUI npcPortrait;

    [Header("Data References")]
    public OutfitManager playerOutfitManager; // To get the player's outfit

    [Header("Typing Effect")]
    public float typingSpeed = 0.04f;

    private Coroutine typingCoroutine;
    private string fullSentence;
    private NPCController currentNPC;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
        CurrentState = DialogueState.Idle;
    }

    void Start()
    {
        if (dialogueUIParent != null) dialogueUIParent.SetActive(false);
    }

    // Modified to accept the NPC so we can get their outfit
    public void StartDialogue(DialogueNode node, NPCController npc)
    {
        currentNPC = npc;
        dialogueUIParent.SetActive(true);

        // Display the outfits
        // --- THIS LINE IS NOW FIXED ---
        playerPortrait.DisplayOutfit(new List<ClothingItem>(playerOutfitManager.currentOutfit.Values));
        npcPortrait.DisplayOutfit(currentNPC.GetCurrentOutfit());

        ShowNode(node);
    }

    // A new helper function to show a node's content
    private void ShowNode(DialogueNode node)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(node.sentence));
        DisplayOptions(node);
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        CurrentState = DialogueState.Idle;
        dialogueUIParent.SetActive(false);
        currentNPC = null;
    }

    public void SkipTyping()
    {
        if (CurrentState != DialogueState.Typing) return;
        StopCoroutine(typingCoroutine);
        dialogueText.text = fullSentence;
        CurrentState = DialogueState.Displaying;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        CurrentState = DialogueState.Typing;
        dialogueText.text = "";
        fullSentence = sentence;
        foreach (char letter in fullSentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        CurrentState = DialogueState.Displaying;
    }

    private void DisplayOptions(DialogueNode node)
    {
        // Hide all buttons first
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        // If there are options, set up and show the corresponding buttons
        if (node.options != null && node.options.Count > 0)
        {
            for (int i = 0; i < node.options.Count; i++)
            {
                // Make sure we don't try to access a button that doesn't exist
                if (i < optionButtons.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    // Update the button's text
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i].responseText;
                }
            }
        }
    }
}