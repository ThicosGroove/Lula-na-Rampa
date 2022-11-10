using UnityEngine;
using UnityEngine.SceneManagement;


public class LoginScenceManager : MonoBehaviour
{
    public void ClickOnPlayOffline()
    {
        SceneManager.LoadScene(Const.GAME_SCENE);
    }
}
