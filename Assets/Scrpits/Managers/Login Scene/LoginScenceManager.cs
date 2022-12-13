using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginScenceManager : MonoBehaviour
{
    [SerializeField] Image keepMeImage;

    bool keepMe = false;

    private void Start()
    {
        keepMe = SaveManager.instance.LoadFile()._keepMeConnected;

        if (keepMe)
        {
            keepMeImage.color = Color.white; 
        }
        else
        {
            keepMeImage.color = Color.clear;
        }

        GameManager.instance.UpdateSceneState(SceneState.LOGIN);
    }

    public void ClickOnPlayOffline()
    {
        SceneManager.LoadScene(Const.GAME_SCENE);
    }

    public void ClickOnKeepMeConnected()
    {
        keepMe = !keepMe;

        if (keepMe)
        {
            keepMeImage.color = Color.white;
        }
        else
        {
            keepMeImage.color = Color.clear;
        }
    }
}
