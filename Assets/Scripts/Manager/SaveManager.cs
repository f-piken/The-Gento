using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : Singleton<SaveManager>
{
    public PlayerData playerData;

    [SerializeField]
    private SaveBinary sav;
    private const string FILE_NAME = "sav";

    public bool IsNewPlayer
    {
        get
        {
            return string.IsNullOrEmpty(playerData.PlayerName);
        }
    }

    public override void Initialization()
    {
        playerData = Load();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        sav.Save(FILE_NAME, playerData);
    }

    [ContextMenu("Load")]

    public PlayerData Load()
    {
        PlayerData loaded = (PlayerData)sav.Load(FILE_NAME);
        if (loaded == null)
        {
            return new PlayerData();
        }

        return loaded;
    }
}