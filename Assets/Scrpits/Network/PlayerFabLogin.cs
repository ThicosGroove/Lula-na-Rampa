using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    private string userName;
    private string userEmail;
    private string userPassword;

    public GameObject loginPanel;
    public GameObject addLoginPanel;
    public GameObject recoverButton;
    public GameObject mobileAutomaticPanel;
    public GameObject gameModePanel;


    public void Start()
    {
        CloseAllPanels();


        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = Const.TITLE_ID; // Please change this value to your own titleId from PlayFab Game Manager
        }

        //PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey(Const.EMAIL))
        {
            userEmail = PlayerPrefs.GetString(Const.EMAIL);
            userPassword = PlayerPrefs.GetString(Const.PASSWORD);
            userName = PlayerPrefs.GetString(Const.USERNAME);

            var request = new LoginWithEmailAddressRequest
            {
                Email = userEmail,
                Password = userPassword,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var requestAndroid = new LoginWithAndroidDeviceIDRequest
                {
                    AndroidDeviceId = ReturnMobileID(),
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true },
                    CreateAccount = true
                };

                PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                var requestIOS = new LoginWithIOSDeviceIDRequest
                {
                    DeviceId = ReturnMobileID(),
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true },
                    CreateAccount = true
                };

                PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
            }
        }

        loginPanel.SetActive(true);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        CloseAllPanels();

        PlayerPrefs.SetString(Const.EMAIL, userEmail);
        PlayerPrefs.SetString(Const.PASSWORD, userPassword);
        PlayerPrefs.SetString(Const.USERNAME, userName);
        PlayerPrefs.SetString(Const.PLAYER_ID, result.PlayFabId);

        var nameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(nameRequest, OnDisplayNameUpdate, OnDisplayNameUpdateError);

        //SceneManager.LoadScene(Const.GAME_SCENE);
        GoToGameModePanel();
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        CloseAllPanels();

        PlayerPrefs.SetString(Const.EMAIL, userEmail);
        PlayerPrefs.SetString(Const.PASSWORD, userPassword);
        PlayerPrefs.SetString(Const.USERNAME, userName);
        PlayerPrefs.SetString(Const.PLAYER_ID, result.PlayFabId);

        var nameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(nameRequest, OnDisplayNameUpdate, OnDisplayNameUpdateError);

        //SceneManager.LoadScene(Const.GAME_SCENE);
        GoToGameModePanel();
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        loginPanel.SetActive(false);

        PlayerPrefs.SetString(Const.PLAYER_ID, result.PlayFabId);

        if (result.InfoResultPayload.PlayerProfile != null)
        {
            userName = result.InfoResultPayload.PlayerProfile.DisplayName;

            GoToGameModePanel();
            //SceneManager.LoadScene(Const.GAME_SCENE);
        }

        if (!PlayerPrefs.HasKey(Const.USERNAME))
        {
            CloseAllPanels();
            mobileAutomaticPanel.SetActive(true);
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GetUserName(string userNameInput)
    {
        userName = userNameInput;
    }

    public void GetUserEmail(string emailInput)
    {
        userEmail = emailInput;
    }

    public void GetUserPassword(string passwordInput)
    {
        userPassword = passwordInput;
    }

    public void OnClickLoginPC()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = userEmail,
            Password = userPassword,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);      
    }

    public void OnCLickLoginMobile()
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnDisplayNameUpdateError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.LogWarning("NOME CERTO " + result.DisplayName);
    }

    void OnDisplayNameUpdateError(PlayFabError error)
    {
        Debug.LogWarning("Nao conseguiu atualizar o nome " + error.ErrorMessage);
    }

    public void ClickOnPlayOffline()
    {
        //SceneManager.LoadScene(Const.GAME_SCENE);
        GoToGameModePanel();
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    public void OpenAddLogin()
    {
        addLoginPanel.SetActive(true);
    }

    public void OnClickAddLogin()
    {
        var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnRegisterFailure);
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString(Const.EMAIL, userEmail);
        PlayerPrefs.SetString(Const.PASSWORD, userPassword);
        PlayerPrefs.SetString(Const.USERNAME, userName);
        loginPanel.SetActive(false);
        recoverButton.SetActive(false);

        GoToGameModePanel();
        //SceneManager.LoadScene(Const.GAME_SCENE);
    }

    private void GoToGameModePanel()
    {
        gameModePanel.SetActive(true);
    }

    private void CloseAllPanels()
    {
        loginPanel.SetActive(false);
        addLoginPanel.SetActive(false);
        mobileAutomaticPanel.SetActive(false);
        recoverButton.SetActive(false);
        gameModePanel.SetActive(false);
    }
}
