using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBase {

    public static GameManager Instance = null;

    void Awake()
    {
        Instance = this;
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case GameEvent.GAME_START:
                MsgCenter.Instance.Dispatch(AreaCode.CHARACTER,PlayerEvent.PLAYER_INITDATA,message);
                break;
            case GameEvent.GAME_COLOR:
                MsgCenter.Instance.Dispatch(AreaCode.CHARACTER,PlayerEvent.PLAYER_IDENTIFY,message);
                break;
            case GameEvent.GAME_OVER:
                break;
            default:
                break;
        }
    }

    
}
