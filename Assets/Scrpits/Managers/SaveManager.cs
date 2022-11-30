using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public PlayerData playerData;

    private void Start()
    {
        playerData = new PlayerData();

        if (!File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {
            playerData.isNormalMode = true;
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.dataPath + Const.SAVE_FILE_PATH, json);
    }

    public PlayerData LoadFile()
    {
        if (Const.SAVE_FILE_PATH == null) return null;

        string json = File.ReadAllText(Application.dataPath + Const.SAVE_FILE_PATH);
        PlayerData loadPlayerData = JsonUtility.FromJson<PlayerData>(json);
        return loadPlayerData;
    }

    public class PlayerData
    {
        public bool isNormalMode;
    }
}
