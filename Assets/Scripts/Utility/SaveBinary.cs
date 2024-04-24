using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveBinary : MonoBehaviour
{
    private string _saveFolder
    {
        get => Application.persistentDataPath + SAVE_FOLDER;
    }

    private const string SAVE_FOLDER = "/saves";

    private const string SAVE_EXT = ".hash";

    public void Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(_saveFolder)) //Folder doesn't exist
        {
            Directory.CreateDirectory(_saveFolder); //Creating the folder
        }

        string path = _saveFolder + "/" + saveName + SAVE_EXT;

        FileStream file = File.Create(path);
        formatter.Serialize(file, saveData);
        file.Close();
#if UNITY_EDITOR
        Debug.Log($"SAVE: {path}");
#endif
    }

    public object Load(string fileName)
    {
        if (!IsFileExist(fileName))
        {
#if UNITY_EDITOR
            Debug.LogError($"SAVE: file doesn't exist with filename: {fileName}");
#endif
            return null;
        }

        string path = _saveFolder + "/" + fileName + SAVE_EXT;

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);
        try
        {
            object saveFile = formatter.Deserialize(file);
            file.Close();
            return saveFile;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SAVE: Failed to load file at: \n{path}\n{ex.Message}");
            file.Close();
            return null;
        }
    }

    public bool IsFileExist(string fileName)
    {
        string path = _saveFolder + "/" + fileName + SAVE_EXT;
        if (!File.Exists(path)) //File doesn't exists
        {
            return false;
        }

        return true;
    }

    public BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        //TODO (if needed) - Serialization Surrogates

        return formatter;
    }
}