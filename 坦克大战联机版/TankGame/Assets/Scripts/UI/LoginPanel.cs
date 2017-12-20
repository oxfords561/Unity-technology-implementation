using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.LOGIN_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.LOGIN_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button loginButton;
    private Button closeButton;
    private InputField usernameInput;
    private InputField passwordInput;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg = new SocketMsg();

    void Start()
    {
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        usernameInput = transform.Find("UsernameInput").GetComponent<InputField>();
        passwordInput = transform.Find("PasswordInput").GetComponent<InputField>();

        loginButton.onClick.AddListener(loginClick);
        closeButton.onClick.AddListener(closeClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();

        //面板需要默认隐藏
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        loginButton.onClick.RemoveListener(loginClick);
        closeButton.onClick.RemoveListener(closeClick);
    }

    /// <summary>
    /// 登录按钮的点击事件处理
    /// </summary>
    private void loginClick()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            promptMsg.Change("登录的用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(passwordInput.text))
        {
            promptMsg.Change("登录的密码不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        if (passwordInput.text.Length < 4 || passwordInput.text.Length > 16)
        {
            promptMsg.Change("登录的密码长度不合法，应该在4-16个字符之内！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        UserDto dto = new UserDto(usernameInput.text, passwordInput.text);
        socketMsg.Change(OpCode.User, UserCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);

        //本地记录用户的相关信息
        PlayerPrefs.SetString("username",usernameInput.text);
        PlayerPrefs.SetString("password", passwordInput.text);
    }

    private void closeClick()
    {
        setPanelActive(false);
    }

}
