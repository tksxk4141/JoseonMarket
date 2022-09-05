using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using UnityEngine.SceneManagement;
using TMPro;
using Michsky.UI.Shift;

public class cshLogin : MonoBehaviour
{
    public GameObject Screen;
    public GameObject ID_Input;
    public GameObject PW_Input;
    public GameObject RegisterID_Input;
    public GameObject RegisterPW_Input;
    public GameObject Email_Input;

    private string username;
    private string password;
    private string email;
    private string rg_username;
    private string rg_password;

    // Use this for initialization
    void Start()
    {
        PlayFabSettings.TitleId = "2C770";
    }

    public void ID_value_Changed()
    {
        username = ID_Input.GetComponent<InputField>().text;
        rg_username = RegisterID_Input.GetComponent<InputField>().text;
    }

    public void PW_value_Changed()
    {
        password = PW_Input.GetComponent<InputField>().text;
        rg_password = RegisterPW_Input.GetComponent<InputField>().text;
    }

    public void Email_value_Changed()
    {
        email = Email_Input.GetComponent<InputField>().text;
    }

    public void Login()
    {
        var request = new LoginWithPlayFabRequest {
            Username = username,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true, // 이 옵션으로 DisplayName,
                    //ShowAvatarUrl = true  // 이 옵션으로 AvatarUrl을 가져올 수 있다.
                },
                //- 이 옵션으로 DisplayName, AvatarUrl을 가져올 수 있다.
                //GetPlayerStatistics = true, //- 이 옵션으로 통계값(순위표에 관여하는)을 불러올 수 있다.
                GetUserData = true, //- 이 옵션으로 < 플레이어 데이터(타이틀) >값을 불러올 수 있다.
            }
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }   

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Username = rg_username, Password = rg_password, Email = email, DisplayName = rg_username };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("로그인 성공");
        cshLoginValue.username = result.InfoResultPayload.PlayerProfile.DisplayName;
        SceneManager.LoadScene(1);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("로그인 실패");
        Debug.LogWarning(error.GenerateErrorReport());
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("가입 성공");
        Screen.GetComponent<Animator>().Play("Sign Up to Login");
    }

    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("가입 실패");
        Debug.LogWarning(error.GenerateErrorReport());
    }
    /*
    void SendCustomAccountRecoveryEmail(string emailAddress, string emailTemplateId)
    {
        var request = new SendCustomAccountRecoveryEmailRequest
        {
            Email = emailAddress,
            EmailTemplateId = emailTemplateId
        };

        PlayFabServerAPI.SendCustomAccountRecoveryEmail(request, res =>
        {
            Debug.Log("An account recovery email has been sent to the player's email address.");
        }, FailureCallback);
    }
    */
    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    void Update()
    {

    }
}
