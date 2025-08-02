using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Fashion/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string sentence;
    public List<DialogueOption> options = new List<DialogueOption>();
}

[System.Serializable]
public class DialogueOption
{
    public string responseText;
    public DialogueNode nextNode;
}
