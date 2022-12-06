using UnityEngine;
using TMPro;

public class BannerBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Text displayBaneer;

    string[] bannerText =
        {
        "TÁ NA HORA DO JAIR.. JÁ IR EMBORA!!",
        "PAREM DE MIMIMI",
        "MI MI MI",
        "TERRA É PLANA !!",
        "SOMOS GADO",
        "CLOROQUINERS",
        "GOLPE MILITAR",
        "PATRIOTÁRIOS",
        "NÃO SOU COVEIRO",
        "PERDEMOS, MANÉ",
        "INTERVENÇÃO PSIQUIÁTRICA JÁ",
        "IDOSOS DE ZAP",
        "TIOZÃO DO ZAP",
        "TIAZONA DO ZAP",
        "RECEBI NO ZAP",
        "SÓ SABEMOS O QUE TÁ NO ZAP",
        "SÓ O VIAGRA SALVA",
        "SÓ MAIS 72 HORAS, AGUARDEM!",
        "PENDRIVE DAS ARÁBIAS",
        "SUPREMO É O POVO, NÃO PERA!",
        "NÃO VÃO COMER O MEU CACHORRO",
        "DEITA NA BR E CHORA",
        "OS CAVALEIROS DO AGUARDEM",
        "BANHEIRO UNISEX SÓ SE FOR QUÍMICO",
        "51 IMÓVEIS NO DINHEIRO VIVO?!?!",
        "GOSTAMOS SECRETAMENTE DA ANITTA",
        "AQUI SERVIMOS O SHEIK DO PENDRIVE",
        "S.O.S. E.T.",
        "INTERVENÇÃO ALIENÍGENA!",
        "A CRISE É ESTÉTICA",
        "FAMÍLIA TRADICIONAL BRASILEIRA E MINHA AMANTE",
        "NÃO TEMOS VERGONHA NA CARA",
        "NÃO ESTUDAMOS HISTÓRIA",
        "SEM PRESSÃO, AQUI É XANDÃO",
        "BURROS COM FORÇA (ARMADA)",
        "MILÍCIA SUA LINDA",
        "MAMADEIRA DE PIROCA",
        "CANETA BIC NA MÃO E OZÔNIO NO TOBA",
        "BROCHAMOS",
        "BROCHAS E CORNOS",
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
