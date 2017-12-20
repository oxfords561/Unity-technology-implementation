using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : UIBase
{

	void Awake () {
        Bind(UIEvent.GAMEOVER_PANEL_ACTIVE);
        setPanelActive(false);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.GAMEOVER_PANEL_ACTIVE:
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

        Dispatch(AreaCode.UI, UIEvent.READY_PANEL_ACTIVE, true);
    }
}
