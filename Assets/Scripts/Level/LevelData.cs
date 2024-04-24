using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Level", menuName = "Salwa/Level")]

public class LevelData : ScriptableObject
{
    [Header("Dialog")]
    public DialogData dialog;

    [Header("Enemy")]

    public CharacterData[] enemies;
    //public CharacterData player;

    [Header("Reward")]
    public RewardData[] Rewards;
}