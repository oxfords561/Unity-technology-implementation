using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : HandlerBase {

    private PromptMsg promptMsg = new PromptMsg();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case GameCode.GAMESTART_SRES:
                GameStart();
                break;
            case GameCode.GAMEVICTORY:
                GameVictory();
                break;
            case GameCode.GAMEOVER_SRES:
                GameOver(value);
                break;
            default:
                break;
        }
    }


    private void GameStart()
    {
        //1、游戏开始 关闭准备面板
        Dispatch(AreaCode.UI, UIEvent.READY_PANEL_ACTIVE, false);
        promptMsg.Change("游戏开始了", Color.green);
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);

        //2、通知游戏模块开始游戏初始化
        Dispatch(AreaCode.GAME, GameEvent.GAME_START, null);
    }

    private void GameOver(object value)
    {
        Dispatch(AreaCode.CHARACTER, PlayerEvent.PLAYER_DESTROY, value);
    }

    private void GameVictory()
    {
        Dispatch(AreaCode.UI,UIEvent.GAMEVICTORY_PANEL_ACTIVE, true);
    }
}
