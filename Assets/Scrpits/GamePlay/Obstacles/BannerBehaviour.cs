using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BannerBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Text displayBaneer;

    string[] bannerText =
        {
        "BURROS COM FOR�A (ARMADA)",
        "S.O.S. E.T.",
        "PAREM DE MIMIMI",
        "TERRA PLANA",
        "SOMOS GADO",
        "CLOROQUINERS",
        "GOLPE MILITAR",
        "PATRIOT�RIOS",
        "N�O SOU COVEIRO",
        "BROCHAS E CORNOS",
        "PERDEMOS, MAN�",
        "IDOSOS DE ZAP",
        "INTERVEN��O PSIQUI�TRICA J�",
        "TAVA NO ZAP",
        "CANETA BIC NA M�O E OZ�NIO NO TOBA",
        "S� O VIAGRA SALVA",
        "MAIS 72 HORAS, AGUARDEM!",
        "PENDRIVE DAS AR�BIAS"
    };


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
