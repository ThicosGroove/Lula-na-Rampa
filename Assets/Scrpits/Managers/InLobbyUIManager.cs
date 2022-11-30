using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class InLobbyUIManager : MonoBehaviour
{
    public GameObject gameModePanel;
    public GameObject logInPanel;
    public GameObject playButton;

    public GameObject useName_Text; 
    public GameObject email_Text; 
    public GameObject password_Text; 

    private void Start()
    {
        playButton.SetActive(false);

        if (File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {     
            useName_Text.GetComponent<TMP_InputField>().text = SaveManager.Instance.LoadFile()._userName;
            email_Text.GetComponent<TMP_InputField>().text = SaveManager.Instance.LoadFile()._email;
            password_Text.GetComponent<TMP_InputField>().text = SaveManager.Instance.LoadFile()._password;
        }
    }

    public void ClickOnClassicMode()
    {
        SaveManager.Instance.playerData._isNormalMode = true;
        SaveManager.Instance.SaveData();
        playButton.SetActive(true);
    }

    public void ClickOnInfinityMode()
    {
        SaveManager.Instance.playerData._isNormalMode = false;
        SaveManager.Instance.SaveData();
        playButton.SetActive(true);
    }

    public void ClickOnPlayButton()
    {
        SceneManager.LoadScene(Const.GAME_SCENE);
    }

    public void ClickOnLogOutButton()
    {
        gameModePanel.SetActive(false);
        logInPanel.SetActive(true);
        SaveManager.Instance.playerData._keepMeConnected = false;
        SaveManager.Instance.SaveData();
    }

    public void CLickOnKeepMeConnected()
    {
        SaveManager.Instance.playerData._keepMeConnected = !SaveManager.Instance.playerData._keepMeConnected;
        SaveManager.Instance.SaveData();
    }
}
