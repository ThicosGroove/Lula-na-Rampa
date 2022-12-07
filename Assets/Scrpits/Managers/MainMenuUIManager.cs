using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject playButton;

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
        SaveManager.Instance.playerData._keepMeConnected = false;
        SaveManager.Instance.SaveData();

        SceneManager.LoadScene(Const.HOME_SCENE);
    }
}
