using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BannerBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Text displayBaneer;

    string[] bannerText =
        {
        "BURRO COM FOR�A (ARMADA)",
        "S.O.S. E.T",
        "PAREM DE MIMIMI",
        "TERRA PLANA",
        "SOMOS GADO",
        "CLOROQUINERS",
        "GOLPE MILITAR",
        "PATRIOT�RIOS"};


    private void Start()
    {
        displayBaneer.text = ChooseBannerText();
    }


    private string ChooseBannerText()
    {
        string randomPhrase = bannerText[Random.Range(0, bannerText.Length)];

        return randomPhrase;
    }

}
