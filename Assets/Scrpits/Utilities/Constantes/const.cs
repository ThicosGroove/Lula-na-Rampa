using UnityEngine;

public static class Const
{
    public static string TITLE_ID = "E5C38";

    //PLAYER DATA
    public static string EMAIL = "EMAIL";
    public static string PASSWORD = "PASSWORD";
    public static string USERNAME = "USERNAME";
    public static string PLAYER_ID = "PLAYER_ID";

    //Scenes 
    public static int TITLE_SCENE = 0;
    public static int LOGIN_SCENE = 1;
    public static int MAIN_MENU_SCENE = 2;
    public static int GAME_SCENE = 3;

    public static string PLAYER_TAG = "Player";
    public static string OBSTACLE_TAG = "Obstacle";
    public static string STAR_TAG = "Star";
    public static string REWARD_TAG = "Reward";

    //Game Play
    public static int SCORE_PER_COLLECTABLE = 1;
    public static int PLAYER_INITIAL_LANE = 1;
    public static float LANE_DISTANCE = 15f;
    public static float PLAYER_ROTATION_MOVE = 45f;

    //Layers
    public static string LAYER_FAIXA = "Faixa";

    //Audio
    public static string MASTER_MIXER = "MasterAudioMixer";
    public static string BG_MIXER = "BGAudioMixer";
    public static string SFX_MIXER = "SFXAudioMixer";

    //Animation
    public static string RUN_ANIMATION = "IsRunning_Anim";
    public static string JUMP_ANIMATION = "IsJump_Anim";
    public static string ROLL_ANIMATION = "IsRoll_Anim";
    public static string WIN_ANIMATION = "HasWin_Anim";
    public static string JUMP_SPEED_ANIMATION = "JumpSpeed_Anim";
    public static string ROLL_SPEED_ANIMATION = "RollSpeed_Anim";



    // Save Files
    public static string GetSaveFilePath()
    {
#if UNITY_STANDALONE_WIN
            return Application.dataPath + "/saveFile.json";
#else
        return Application.persistentDataPath + "/saveFile.json";
#endif
    }

    // PLAYFAB 
    public static string SCOREBOARD_NAME = "Global Scoreboard";
}