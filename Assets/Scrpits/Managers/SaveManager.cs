using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public PlayerData playerData;

    #region SINGLETON PATTERN
    public static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SaveManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("SaveManager");
                    instance = container.AddComponent<SaveManager>();
                }
            }

            return instance;
        }
    }
    #endregion


    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }



    private void Start()
    {
        playerData = new PlayerData();

        if (!File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {
            playerData._isNormalMode = false;
            playerData._userName = null;
            playerData._email = null;
            playerData._playerID = null;
            playerData._keepMeConnected = false;
            playerData._masterMusicVolume = -10f;
            playerData._backgroundVolume = -10f;
            playerData._sfxVolume = -10f;
            playerData._cameraPosition = new Vector3(0f, 26f, -47);
            SaveData();
        }

        playerData._isNormalMode = LoadFile()._isNormalMode;
        playerData._userName = LoadFile()._userName;
        playerData._email = LoadFile()._email;
        playerData._playerID = LoadFile()._playerID;
        playerData._keepMeConnected = LoadFile()._keepMeConnected;
        playerData._masterMusicVolume = LoadFile()._masterMusicVolume;
        playerData._backgroundVolume = LoadFile()._backgroundVolume;
        playerData._sfxVolume = LoadFile()._sfxVolume;
        playerData._cameraPosition = LoadFile()._cameraPosition;
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
        public bool _isNormalMode;

        public string _userName;
        public string _email;
        public string _playerID;

        public bool _keepMeConnected;

        public float _masterMusicVolume;
        public float _backgroundVolume;
        public float _sfxVolume;

        public Vector3 _cameraPosition;
        public int _musicIndex;
        public int _cameraImageIndex;
    }
}
