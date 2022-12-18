using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class PlayFabLoginManager : MonoBehaviour
{
    private string userName;
    private string userEmail;

    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject addLoginPanel;
    [SerializeField] GameObject recoverButton;
    [SerializeField] GameObject mobileAutomaticPanel;
    [SerializeField] TMP_Text error_text;

    public void Start()
    {
        CloseAllPanels();
        loginPanel.SetActive(true);

        error_text.text = "";

        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = Const.TITLE_ID; // Please change this value to your own titleId from PlayFab Game Manager
        }

        if (File.Exists(Application.dataPath + Const.SAVE_FILE_PATH))
        {
            userName = SaveManager.Instance.LoadFile()._userName;
            userEmail = SaveManager.Instance.LoadFile()._email;

            var request = new LoginWithEmailAddressRequest
            {
                Email = userEmail,
                Password = Const.PASSWORD,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
            };


            if (SaveManager.Instance.LoadFile()._keepMeConnected)
            {
                Debug.LogWarning("Mantenhame conectado");

                PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
            }
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
    }

    #region Connection
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        //CloseAllPanels();

        SaveManager.Instance.playerData._userName = userName;
        SaveManager.Instance.playerData._email = userEmail;
        SaveManager.Instance.playerData._playerID = result.PlayFabId;
        SaveManager.Instance.SaveData();

        var nameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(nameRequest, OnDisplayNameUpdate, OnDisplayNameUpdateError);

        GoToGameModePanel();
    }

    private void OnLoginSuccess(LoginResult result)
    {
        //CloseAllPanels();

        SaveManager.Instance.playerData._userName = userName;
        SaveManager.Instance.playerData._email = userEmail;
        SaveManager.Instance.playerData._playerID = result.PlayFabId;
        SaveManager.Instance.SaveData();

        var nameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(nameRequest, OnDisplayNameUpdate, OnDisplayNameUpdateError);

        GoToGameModePanel();
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        loginPanel.SetActive(false);

        SaveManager.Instance.playerData._playerID = result.PlayFabId;
        SaveManager.Instance.SaveData();

        if (result.InfoResultPayload.PlayerProfile != null)
        {
            userName = result.InfoResultPayload.PlayerProfile.DisplayName;

            GoToGameModePanel();
        }

        if (SaveManager.Instance.LoadFile()._userName != null)
        {
            CloseAllPanels();
            mobileAutomaticPanel.SetActive(true);
        }
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result)
    {

        SaveManager.Instance.playerData._userName = userName;
        SaveManager.Instance.playerData._email = userEmail;
        SaveManager.Instance.SaveData();

        loginPanel.SetActive(false);
        recoverButton.SetActive(false);

        GoToGameModePanel();
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        error_text.text = "Email não encontrado\n Cadastre-se com esse e-mail\n" + error.ErrorMessage;
        Debug.LogWarning(error.ErrorMessage);
    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        error_text.text = error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        error_text.text = "Nome ou Email já exitem\n " + error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion Connection


    #region Inputs
    public void GetUserName(string userNameInput)
    {
        userName = userNameInput;
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void GetUserEmail(string emailInput)
    {
        userEmail = emailInput;
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.EmailAddress);
    }
    #endregion Inputs

    #region Buttons
    public void OnClickRegistration()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = userName,
            Email = userEmail,
            Password = Const.PASSWORD,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    public void OnClickLoginPC()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = userEmail,
            Password = Const.PASSWORD,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void OnCLickLoginMobile()
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = userName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnDisplayNameUpdateError);
    }

    public void OnClickAddLogin()
    {
        var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = Const.PASSWORD, Username = userName };
        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnRegisterFailure);
    }

    public void ClickOnPlayOffline()
    {
        GoToGameModePanel();
    }
    #endregion Buttons


    #region Displays
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.LogWarning("NOME CERTO " + result.DisplayName);
    }

    void OnDisplayNameUpdateError(PlayFabError error)
    {
        Debug.LogWarning("Nao conseguiu atualizar o nome " + error.ErrorMessage);
    }

    public void OpenAddLogin()
    {
        addLoginPanel.SetActive(true);
    }
    #endregion Displays


    private void GoToGameModePanel()
    {
        SceneManager.LoadScene(Const.MAIN_MENU_SCENE);
    }

    private void CloseAllPanels()
    {
        loginPanel.SetActive(false);
        addLoginPanel.SetActive(false);
        mobileAutomaticPanel.SetActive(false);
        recoverButton.SetActive(false);
    }
}
