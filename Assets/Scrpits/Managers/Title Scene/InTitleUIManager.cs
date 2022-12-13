using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InTitleUIManager : MonoBehaviour
{
    public void ClickOnTitleScreen()
    {
        SceneManager.LoadScene(Const.LOGIN_SCENE);
    }

}
