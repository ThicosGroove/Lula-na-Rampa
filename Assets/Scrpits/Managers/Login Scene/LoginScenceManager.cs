using UnityEngine;
using UnityEngine.SceneManagement;


public class LoginScenceManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.UpdateSceneState(SceneState.LOGIN);
    }

    public void ClickOnPlayOffline()
    {
        SceneManager.LoadScene(Const.GAME_SCENE);
    }
}
