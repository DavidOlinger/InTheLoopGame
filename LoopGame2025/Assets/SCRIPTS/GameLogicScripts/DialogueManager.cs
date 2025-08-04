using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public enum DialogueState { Idle, Typing, Displaying }
    public DialogueState CurrentState { get; private set; }
    public static DialogueManager instance;

    [Header("UI Components")]
    public GameObject dialogueUIParent;
    public TextMeshProUGUI dialogueText;
    public Button[] optionButtons;

    [Header("Portrait References")]
    public GameObject playerPortraitObject; // Reference to the shared portrait object
    public PortraitUI npcPortrait;
    public Sprite playerBodySprite;

    [Header("Data References")]
    public OutfitManager playerOutfitManager;

    [Header("Typing Effect")]
    public float typingSpeed = 0.04f;

    private Coroutine typingCoroutine;
    private string fullSentence;
    private NPCController currentNPC;
    private PortraitUI playerPortraitUI;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
        CurrentState = DialogueState.Idle;
    }

    void Start()
    {
        playerPortraitUI = playerPortraitObject.GetComponent<PortraitUI>();
        dialogueUIParent.SetActive(false);
    }

    public void StartDialogue(DialogueNode node, NPCController npc)
    {
        currentNPC = npc;
        dialogueUIParent.SetActive(true);
        playerPortraitObject.SetActive(true);

        playerPortraitUI.DisplayOutfit(playerBodySprite, new List<ClothingItem>(playerOutfitManager.currentOutfit.Values));
        npcPortrait.DisplayOutfit(npc.portraitBodySprite, npc.GetCurrentOutfit());

        ShowNode(node);
    }

    public void ShowNode(DialogueNode node)
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
        playerPortraitObject.SetActive(false);
        currentNPC = null;
    }

    public void SkipTyping()
    {
        if (CurrentState != DialogueState.Typing) return;
        StopCoroutine(typingCoroutine);
        dialogueText.text = fullSentence;
        CurrentState = DialogueState.Displaying;
    }

    private string ReplacePlaceholders(string text)
    {
        if (GameManager.instance == null) return text;
        text = text.Replace("<HOT_HAT>", GameManager.instance.HotHat.itemName);
        text = text.Replace("<HOT_SHIRT>", GameManager.instance.HotShirt.itemName);
        text = text.Replace("<HOT_PANTS>", GameManager.instance.HotPants.itemName);
        text = text.Replace("<PLAYER_NAME>", "Darling");
        return text;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        CurrentState = DialogueState.Typing;
        dialogueText.text = "";
        fullSentence = ReplacePlaceholders(sentence);
        foreach (char letter in fullSentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        CurrentState = DialogueState.Displaying;
    }

    private void DisplayOptions(DialogueNode node)
    {
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (node.options != null && node.options.Count > 0)
        {
            for (int i = 0; i < node.options.Count; i++)
            {
                if (i < optionButtons.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i].responseText;
                }
            }
        }
    }
}