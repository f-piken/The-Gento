using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Salwa/Dialog")]

public class DialogData : ScriptableObject
{
    public Sprite Background;
    public CharacterData LeftSpeaker;
    public DialogText[] Dialog;
}

[System.Serializable]
public class DialogText
{
    public CharacterData Speaker;

    [TextArea(2, 5)]
    public string Text;
}