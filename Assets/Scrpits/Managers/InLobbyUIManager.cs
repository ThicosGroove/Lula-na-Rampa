using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class InLobbyUIManager : MonoBehaviour
{
    public GameObject useName_Text; 
    public GameObject email_Text; 
    public GameObject password_Text; 

    private void Start()
    {
        if (File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {     
            useName_Text.GetComponent<TMP_InputField>().text = SaveManager.Instance.LoadFile()._userName;
            email_Text.GetComponent<TMP_InputField>().text = SaveManager.Instance.LoadFile()._email;
            password_Text.GetComponent<TMP_InputField>().text = SaveManager.Instance.LoadFile()._password;
        }
    }

    public void CLickOnKeepMeConnected()
    {
        SaveManager.Instance.playerData._keepMeConnected = !SaveManager.Instance.playerData._keepMeConnected;
        SaveManager.Instance.SaveData();
    }
}
