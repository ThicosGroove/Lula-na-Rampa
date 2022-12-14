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
    [SerializeField] GameObject creditsPanel;

    [Header("Buttons")]
    [SerializeField] GameObject playButton;

    [Header("Volume Setting")]
    [SerializeField] Slider masterMusicSlider;
    [SerializeField] Slider bgSlider;
    [SerializeField] Slider sfxSlider;
    private float Master_Volume;
    private float BG_Volume;
    private float SFX_Volume;

    [Header("Texts")]
    [SerializeField] TMP_Text bgMusic_name;
    [SerializeField] TMP_Text master_Sound_text;
    [SerializeField] TMP_Text BG_Sound_text;
    [SerializeField] TMP_Text SFX_Sound_text;
    [SerializeField] TMP_Text volumeText;

    [Header("Camera Settings")]
    [SerializeField] RawImage image;

    [Header("Options Data")]
    [SerializeField] OptionsSO optionsData;

    [Header("Save Settings")]
    [SerializeField] Image saveIcon;


    MusicManager musicManager;
    int musicIndex;
    int cameraImageIndex ;

    private void Start()
    {
       // GameManager.instance.UpdateSceneState(SceneState.MAIN_MENU);

        CloseAllPanels();
        mainMenuPanel.SetActive(true);

        saveIcon.enabled = false;

        playButton.SetActive(false);
        musicManager = MusicManager.Instance;

        musicIndex = SaveManager.instance.LoadFile()._musicIndex;
        VerifyMusicName(musicIndex);

        cameraImageIndex = SaveManager.instance.LoadFile()._cameraImageIndex;
        image.texture = optionsData.cameraImages[cameraImageIndex];

        master_Sound_text.text = (SaveManager.instance.LoadFile()._masterMusicVolume + 40f).ToString("0.0");
        BG_Sound_text.text = (SaveManager.instance.LoadFile()._backgroundVolume + 40f).ToString("0.0");
        SFX_Sound_text.text = (SaveManager.instance.LoadFile()._sfxVolume + 40f).ToString("0.0");


        Master_Volume = SaveManager.Instance.LoadFile()._masterMusicVolume;
        BG_Volume = SaveManager.Instance.LoadFile()._backgroundVolume;
        SFX_Volume = SaveManager.Instance.LoadFile()._sfxVolume;

        masterMusicSlider.onValueChanged.AddListener(SetMasterVolumeText);
        bgSlider.onValueChanged.AddListener(SetBGVolumeText);
        sfxSlider.onValueChanged.AddListener(SetSFXVolumeText);

        LoadSliderValue();
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

    public void ClickOnLogOutButton()
    {
        SaveManager.Instance.playerData._keepMeConnected = false;
        SaveManager.Instance.SaveData();

        SceneManager.LoadScene(Const.LOGIN_SCENE);
    }
    #endregion Main Menu

    #region Options

    #region Music Options
    public void ClickOnNextMusic()
    {
        musicIndex++;

        if (musicIndex > optionsData.allBG_Music.Length - 1)
        {
            musicIndex = 0;
        }

        VerifyMusicName(musicIndex);

        AudioClip newClip = optionsData.allBG_Music[musicIndex];

        SaveManager.instance.playerData._musicIndex = musicIndex;

        musicManager.ChangeBGMusic(newClip);
    }

    public void ClickOnPreviousMusic()
    {
        musicIndex--;

        if (musicIndex < 0)
        {
            musicIndex = optionsData.allBG_Music.Length - 1;
        }

        VerifyMusicName(musicIndex);

        AudioClip newClip = optionsData.allBG_Music[musicIndex];

        SaveManager.instance.playerData._musicIndex = musicIndex;

        musicManager.ChangeBGMusic(newClip);
    }

    private void LoadSliderValue()
    {
        masterMusicSlider.value = Master_Volume;
        bgSlider.value = BG_Volume;
        sfxSlider.value = SFX_Volume;
    }

    public void SetMasterVolumeText(float volume)
    {
        string volumeText = (volume + 40f).ToString("0.0");
        master_Sound_text.text = volumeText;

        masterMusicSlider.value = volume;
        musicManager.SetMasterVolume(volume);
    }

    public void SetBGVolumeText(float volume)
    {
        string volumeText = (volume + 40f).ToString("0.0");
        BG_Sound_text.text = volumeText;

        bgSlider.value = volume;
        musicManager.SetBGVolume(volume);
    }

    public void SetSFXVolumeText(float volume)
    {
        string volumeText = (volume + 40f).ToString("0.0");
        SFX_Sound_text.text = volumeText;

        sfxSlider.value = volume;
        musicManager.SetSFXVolume(volume);
    }

    private void VerifyMusicName(int index)
    {
        switch (index)
        {
            case 0:
                bgMusic_name.text = "Jingle 1989";
                break;
            case 1:
                bgMusic_name.text = "Ole Ole Olá Lula";
                break;
            case 2:
                bgMusic_name.text = "Jingle 2002 - Bote Fé";
                break;
            case 3:
                bgMusic_name.text = "Ole Ole Olá - Guitarra";
                break;
            case 4:
                bgMusic_name.text = "Jingle 2022";
                break;
            case 5:
                bgMusic_name.text = "Ole Ole Olá\nFabiano TromPetista O Brabo";
                break;
            case 6:
                bgMusic_name.text = "Jingle 2006 - Lula de Novo";
                break;
            default:
                break;
        }
    }
    #endregion Music Options

    #region Camera Options
    public void ClickOnNextCameraSet()
    {
        cameraImageIndex++;

        if (cameraImageIndex > optionsData.cameraImages.Length - 1)
        {
            cameraImageIndex = 0;
        }

        image.texture = optionsData.cameraImages[cameraImageIndex];

        SaveManager.instance.playerData._cameraPosition = optionsData.cameraPosition[cameraImageIndex];
        SaveManager.instance.playerData._cameraRotation = optionsData.cameraRotation[cameraImageIndex];
        SaveManager.instance.playerData._cameraImageIndex = cameraImageIndex;
    }

    public void ClickOnPreviousCameraSet()
    {
        cameraImageIndex--;

        if (cameraImageIndex < 0)
        {
            cameraImageIndex = optionsData.cameraImages.Length - 1;
        }

        image.texture = optionsData.cameraImages[cameraImageIndex];

        SaveManager.instance.playerData._cameraPosition = optionsData.cameraPosition[cameraImageIndex];
        SaveManager.instance.playerData._cameraRotation = optionsData.cameraRotation[cameraImageIndex];
        SaveManager.instance.playerData._cameraImageIndex = cameraImageIndex;
    }


    #endregion Camera Options

    public void ClickOnSaveOptions()
    {
        StartCoroutine(SaveIconFade());
        SaveManager.instance.SaveData();
        Debug.LogWarning("Salvou");

    }

    IEnumerator SaveIconFade()
    {
        saveIcon.enabled = true;
        yield return new WaitForSeconds(1f);
        saveIcon.enabled = false;
    }

    #endregion Options



    void CloseAllPanels()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
}
