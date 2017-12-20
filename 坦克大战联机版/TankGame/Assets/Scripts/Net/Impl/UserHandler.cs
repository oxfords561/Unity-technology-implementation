using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.LOGIN:
                loginResponse((int)value);
                break;
            case UserCode.REGIST_SRES:
                registResponse((int)value);
                break;
            case UserCode.READY_SRES:
                ReadyResponse(value);
                break;
            case UserCode.HOUSEOWNER:
                HouseOwnerResponse();
                break;
            default:
                break;
        }
    }

    private PromptMsg promptMsg = new PromptMsg();

    /// <summary>
    /// 登录响应
    /// </summary>
    private void loginResponse(int result)
    {
        switch (result)
        {
            case 0:
                //跳转场景
                LoadSceneMsg msg = new LoadSceneMsg(1,
                    delegate ()
                    {
                        //TODO
                        //向服务器获取信息
                        Debug.Log("加载完成！");
                    });
                Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
                break;
            case -1:
                promptMsg.Change("账号不存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("账号在线", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("账号密码不匹配", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 注册响应
    /// </summary>
    private void registResponse(int result)
    {
        switch (result)
        {
            case 0:
                promptMsg.Change("注册成功", Color.green);
                //跳转场景
                LoadSceneMsg msg = new LoadSceneMsg(1,
                    delegate ()
                    {
                        //TODO
                        //向服务器获取信息
                        Debug.Log("加载完成！");
                    });
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
                break;
            case -1:
                promptMsg.Change("账号已经存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -2:
                promptMsg.Change("账号输入不合法", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            case -3:
                promptMsg.Change("密码不合法", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 准备 反馈
    /// </summary>
    /// <param name="result"></param>
    private void ReadyResponse(object value)
    {
        //将已经准备好的用户人数同步到客户端
        Dispatch(AreaCode.UI, UIEvent.READY_COUNT_SHOW, value);
    }

    /// <summary>
    /// 房主 功能按钮开放
    /// </summary>
    private void HouseOwnerResponse()
    {
        //将房主功能按钮开放给房主
        Dispatch(AreaCode.UI, UIEvent.HOUSE_OWNER, null);
    }
}
