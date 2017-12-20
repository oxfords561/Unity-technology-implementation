using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase
{
    private Button loginButton;
    private Button registerButton;

    void Start()
    {
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        registerButton = transform.Find("RegisterButton").GetComponent<Button>();

        loginButton.onClick.AddListener(LoginClick);
        registerButton.onClick.AddListener(RegistClick);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        loginButton.onClick.RemoveAllListeners();
        registerButton.onClick.RemoveAllListeners();
    }

    private void LoginClick()
    {
        Dispatch(AreaCode.UI, UIEvent.LOGIN_PANEL_ACTIVE, true);
    }


    private void RegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, true);
    }

}
