using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVictoryPanel : UIBase {

    private SocketMsg socketMsg = new SocketMsg();

    void Awake()
    {
        Bind(UIEvent.GAMEVICTORY_PANEL_ACTIVE);
        setPanelActive(false);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.GAMEVICTORY_PANEL_ACTIVE:
                setPanelActive((bool)message);
                StartCoroutine(ReStartGame());
                break;

            default:
                break;
        }
    }

    IEnumerator ReStartGame()
    {
        yield return new WaitForSeconds(3f);
        setPanelActive(false);

        //删除坦克
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i]);
        }

        Dispatch(AreaCode.UI,UIEvent.READY_PANEL_ACTIVE,true);
    }
}
