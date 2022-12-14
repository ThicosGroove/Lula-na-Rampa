using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Options Data", menuName = "ScriptableObjects/Options", order = 2)]
public class OptionsSO : ScriptableObject
{
    [Header("Music Set")]
    public AudioClip[] allBG_Music;
    public AudioClip[] allSFX_Music;

    [Header("Camera Set")]
    public Vector3[] cameraPosition;
    public Vector3[] cameraRotation;

    [Header("Camera Images")]
    public Texture[] cameraImages;
}
