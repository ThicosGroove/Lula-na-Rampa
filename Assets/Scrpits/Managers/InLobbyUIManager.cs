using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InLobbyUIManager : MonoBehaviour
{
    public GameObject playButton;

    private void Start()
    {
        playButton.SetActive(false);
    }

    public void ClickOnClassicMode()
    {
        SaveManager.Instance.playerData.isNormalMode = true;
        SaveManager.Instance.SaveData();
        playButton.SetActive(true);
    }

    public void ClickOnInfinityMode()
    {
        SaveManager.Instance.playerData.isNormalMode = false;
        SaveManager.Instance.SaveData();
        playButton.SetActive(true);
    }

    public void ClickOnPlayButton()
    {
        SceneManager.LoadScene(Const.GAME_SCENE);
    }

    public void ClickOnLogOutButton()
    {

    }
}
