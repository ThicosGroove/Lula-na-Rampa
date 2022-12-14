using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] OptionsSO optionsData;
    [SerializeField] public AudioMixer mixer;

    private AudioSource audioSourceBG;

    private bool isBG_Muted = false;
    private bool isSFX_Muted;
    private float Master_Volume;
    private float BG_Volume;
    private float SFX_Volume;

    private int musicIndex;

    #region SINGLETON PATTERN
    public static MusicManager instance;
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MusicManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("MusicManager");
                    instance = container.AddComponent<MusicManager>();
                }
            }

            return instance;
        }
    }
    #endregion

    void Awake()
    {
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
        audioSourceBG = GetComponent<AudioSource>();

        musicIndex = SaveManager.instance.LoadFile()._musicIndex;
        audioSourceBG.clip = optionsData.allBG_Music[musicIndex];
        audioSourceBG.Play();

        Master_Volume = SaveManager.Instance.LoadFile()._masterMusicVolume;
        BG_Volume = SaveManager.Instance.LoadFile()._backgroundVolume;
        SFX_Volume = SaveManager.Instance.LoadFile()._sfxVolume;

        //isBG_Muted = SaveManager.Instance.LoadFile().BGMusic_mute;
        //isSFX_Muted = SaveManager.Instance.LoadFile().SFX_mute;

        LoadValues();
    }

    private void Update()
    {
        if (!audioSourceBG.isPlaying)
        {
            int nextMusic = musicIndex + 1;

            if (nextMusic > optionsData.allBG_Music.Length)
            {
                nextMusic = 0;
            }

            audioSourceBG.clip = optionsData.allBG_Music[nextMusic];
            audioSourceBG.Play();
        }
    }

    public void ChangeBGMusic(AudioClip clip)
    {
        audioSourceBG.clip = clip;
        audioSourceBG.Play();
    }

    private void LoadValues()
    {
        SetMasterVolume(Master_Volume);
        SetBGVolume(BG_Volume);
        SetSFXVolume(SFX_Volume);
    }

    public void SetMasterVolume(float value)
    {
        Master_Volume = value;

        if (Master_Volume <= -40f)
        {
            Master_Volume = -80f;
        }

        mixer.SetFloat(Const.MASTER_MIXER, Master_Volume);
        SaveManager.instance.playerData._masterMusicVolume = Master_Volume;
    }

    public void SetBGVolume(float value)
    {
        BG_Volume = value;

        if (BG_Volume <= -40f)
        {
            BG_Volume = -80f;
        }

        mixer.SetFloat(Const.BG_MIXER, BG_Volume);
        SaveManager.instance.playerData._backgroundVolume = BG_Volume;
    }

    public void SetSFXVolume(float value)
    {
        SFX_Volume = value;

        if (SFX_Volume <= -40f)
        {
            SFX_Volume = -80f;
        }

        mixer.SetFloat(Const.SFX_MIXER, SFX_Volume);
        SaveManager.instance.playerData._sfxVolume = SFX_Volume;
    }

    //private void Update()
    //{
    //    if (!isBG_Muted)
    //        UpdateBGVolume();
    //    else
    //        Mute();

    //    if (!isSFX_Muted)
    //        UpdateSFXVolume();
    //    else
    //        Mute();
    //}

    //private void UpdateMasterVolume()
    //{
    //    mixer.SetFloat(Const.MASTER_MIXER, Mathf.Log10(BG_Volume) * 20);

    //}

    //private void UpdateBGVolume()
    //{
    //    mixer.SetFloat(Const.BG_MIXER, Mathf.Log10(BG_Volume) * 20);
    //}

    //private void UpdateSFXVolume()
    //{
    //    mixer.SetFloat(Const.SFX_MIXER, Mathf.Log10(SFX_Volume) * 20);

    //}

    //private void Mute()
    //{
    //    if (isBG_Muted)
    //        mixer.SetFloat(Const.BG_MIXER, -80f);
    //    else
    //        mixer.SetFloat(Const.BG_MIXER, Mathf.Log10(BG_Volume) * 20);


    //    if (isSFX_Muted)
    //        mixer.SetFloat(Const.SFX_MIXER, -80f);
    //    else
    //        mixer.SetFloat(Const.SFX_MIXER, Mathf.Log10(SFX_Volume) * 20);

    //}

    //public void ToggleMuteBGMusic()
    //{
    //    isBG_Muted = !isBG_Muted;
    //}

    //public void ToggleMuteSFX()
    //{
    //    isSFX_Muted = !isSFX_Muted;
    //}

}
