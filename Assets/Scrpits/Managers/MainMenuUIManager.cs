using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject optionsPanel;

    [Header("Buttons")]
    [SerializeField] GameObject playButton;

    [Header("Texts")]
    [SerializeField] TMP_Text bgMusic_name;
    [SerializeField] TMP_Text master_Sound_text;
    [SerializeField] TMP_Text BG_Sound_text;
    [SerializeField] TMP_Text SFX_Sound_text;

    [Header("Volume Setting")]
    [SerializeField] TMP_Text volumeText;
    [SerializeField] Slider volumeSlider;


    MusicManager musicManager;
    int index = 0;

    private void Start()
    {
        CloseAllPanels();
        mainMenuPanel.SetActive(true);


        playButton.SetActive(false);
        musicManager = MusicManager.Instance;

        master_Sound_text.text = (SaveManager.instance.LoadFile()._masterMusicVolume + 40f).ToString("0.0");
        BG_Sound_text.text = (SaveManager.instance.LoadFile()._backgroundVolume + 40f).ToString("0.0");
        SFX_Sound_text.text = (SaveManager.instance.LoadFile()._sfxVolume + 40f).ToString("0.0"); 
    }

    #region Main Menu

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

    public void ClickOnOptionsButton(GameObject panelOut, GameObject panelIn)
    {
        panelOut.SetActive(false);
        panelIn.SetActive(true);
    }

    public void ClickOnLogOutButton()
    {
        SaveManager.Instance.playerData._keepMeConnected = false;
        SaveManager.Instance.SaveData();

        SceneManager.LoadScene(Const.HOME_SCENE);
    }
    #endregion Main Menu

    #region Options
    public void ClickOnNextMusic()
    {
        index++;

        if (index > musicManager.BG_clips.Length - 1)
        {
            index = 0;
        }

        VerifyMusicName(index);

        AudioClip newClip = musicManager.BG_clips[index];

        musicManager.ChangeBGMusic(newClip);
    }

    public void ClickOnPreviousMusic()
    {
        index--;

        if (index < 0)
        {
            index = musicManager.BG_clips.Length - 1;
        }

        VerifyMusicName(index);

        AudioClip newClip = musicManager.BG_clips[index];

        musicManager.ChangeBGMusic(newClip);
    }

    public void SetMasterVolumeText(float volume)
    {
        string volumeText = (volume + 40f).ToString("0.0");

        master_Sound_text.text = volumeText;
    }

    public void SetBGVolumeText(float volume)
    {
        string volumeText = (volume + 40f).ToString("0.0");

        BG_Sound_text.text = volumeText;
    }

    public void SetSFXVolumeText(float volume)
    {
        string volumeText = (volume + 40f).ToString("0.0");

        SFX_Sound_text.text = volumeText;
    }

    private void VerifyMusicName(int index)
    {
        switch (index)
        {
            case 0:
                bgMusic_name.text = "OLE OLE OLÁ LULA";
                break;
            case 1:
                bgMusic_name.text = "TÔ COM SAUDADE DO TEMPO DE LULA";
                break;
            case 2:
                bgMusic_name.text = "PABLITO";
                break;
            default:
                break;
        }
    }


    #endregion Options

    void CloseAllPanels()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }
}
