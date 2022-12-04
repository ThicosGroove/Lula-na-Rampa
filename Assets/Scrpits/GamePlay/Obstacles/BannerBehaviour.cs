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
        "MI MI MI",
        "TERRA � PLANA !!",
        "SOMOS GADO",
        "CLOROQUINERS",
        "GOLPE MILITAR",
        "PATRIOT�RIOS",
        "N�O SOU COVEIRO",
        "BROCHAS E CORNOS",
        "PERDEMOS, MAN�",
        "INTERVEN��O PSIQUI�TRICA J�",
        "IDOSOS DE ZAP",
        "TIOZ�O DO ZAP",
        "TIAZONA DO ZAP",
        "RECEBI NO ZAP",
        "S� SABEMOS O QUE T� NO ZAP",
        "MIL�CIA SUA LINDA",
        "CANETA BIC NA M�O E OZ�NIO NO TOBA",
        "S� O VIAGRA SALVA",
        "S� MAIS 72 HORAS, AGUARDEM!",
        "PENDRIVE DAS AR�BIAS",
        "SUPREMO � O POVO, N�O PERA!",
        "N�O V�O COMER O MEU CACHORRO",
        "T� NA HORA DO JAIR.. J� IR EMBORA!!",
        "DEITA NA BR E CHORA",
        "OS CAVALEIROS DO AGUARDEM",
        "BANHEIRO UNISEX S� SE FOR QU�MICO",
        "51 IM�VEIS NO DINHEIRO VIVO?!?!",
        "GOSTAMOS SECRETAMENTE DA ANITTA",
        "AQUI SERVIMOS O SHEIK DO PENDRIVE",
        "INTERVEN��O ALIEN�GENA!",
        "BROCHAMOS",
        "A CRISE � EST�TICA",
        "FAM�LIA TRADICIONAL BRASILEIRA E MINHA AMANTE",
        "N�O TEMOS VERGONHA NA CARA",
        "N�O ESTUDAMOS HIST�RIA",
        "SEM PRESS�O, AQUI � XAND�O",
        "MAMADEIRA DE PIROCA"

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
