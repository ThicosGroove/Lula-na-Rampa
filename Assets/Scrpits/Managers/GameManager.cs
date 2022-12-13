using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneState
{
    TITLE,
    LOGIN,
    MAIN_MENU,
    GAME
}

public class GameManager : MonoBehaviour
{

    [SerializeField] SceneState sceneState = SceneState.TITLE;

    #region SINGLETON PATTERN
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("GameManager");
                    instance = container.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }
    #endregion


    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void UpdateSceneState(SceneState newSceneState)
    {

        sceneState = newSceneState;

        switch (sceneState)
        {
            case SceneState.TITLE:
                break;
            case SceneState.LOGIN:
                break;
            case SceneState.MAIN_MENU:
                break;
            case SceneState.GAME:
                break;
            default:
                break;
        }
    }


}
