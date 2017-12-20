using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button registerButton;
    private Button closeButton;
    private InputField usernameInput;
    private InputField passwordInput;
    private InputField repasswordInput;

    private PromptMsg promptMsg;


    void Start()
    {
        registerButton = transform.Find("RegisterButton").GetComponent<Button>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        usernameInput = transform.Find("UsernameInput").GetComponent<InputField>();
        passwordInput = transform.Find("PasswordInput").GetComponent<InputField>();
        repasswordInput = transform.Find("RePasswordInput").GetComponent<InputField>();

        closeButton.onClick.AddListener(CloseClick);
        registerButton.onClick.AddListener(RegisterClick);

        promptMsg = new PromptMsg();

        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        closeButton.onClick.RemoveListener(CloseClick);
        registerButton.onClick.RemoveListener(RegisterClick);
    }

    UserDto dto = new UserDto();
    SocketMsg socketMsg = new SocketMsg();

    /// <summary>
    /// 注册按钮的点击事件处理
    /// </summary>
    private void RegisterClick()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            promptMsg.Change("注册的用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(passwordInput.text)
            || passwordInput.text.Length < 4
            || passwordInput.text.Length > 16)
        {
            promptMsg.Change("注册的密码不合法！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(repasswordInput.text)
            || repasswordInput.text != passwordInput.text)
        {
            promptMsg.Change("请确保两次输入的密码一致！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        //UserDto dto = new UserDto(usernameInput.text, passwordInput.text);
        socketMsg.Change(OpCode.User, UserCode.REGIST_CREQ, dto);
        dto.username = usernameInput.text;
        dto.password = passwordInput.text;
        socketMsg.OpCode = OpCode.User;
        socketMsg.SubCode = UserCode.REGIST_CREQ;
        socketMsg.Value = dto;
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    private void CloseClick()
    {
        setPanelActive(false);
    }
}
