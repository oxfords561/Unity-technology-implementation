using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;

public class PlayerManager : ManagerBase {

    public static PlayerManager Instance = null;
    private PlayerDto playerDto_SREC = new PlayerDto();
    [SerializeField]
    private GameObject[] players;
    private GameObject[] playerPos;
    private GameObject[] onlinePlayers = new GameObject[4];
    private int color = 0;
    private string[] playerNames = new string[4] { "RedTank(Clone)", "YellowTank(Clone)", "GreenTank(Clone)", "BlueTank(Clone)" };
    private PromptMsg promptMsg = new PromptMsg();

    void Awake()
    {
        Instance = this;
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case PlayerEvent.PLAYER_INITDATA:
                InitGameData();
                break;
            case PlayerEvent.PLAYER_IDENTIFY:
                if (color == 0)
                    color = (int)message;
                break;
            case PlayerEvent.PLAYER_SYNC_POS_ROT:
                SyncPositionAndRotation_SREC(message);
                break;
            case PlayerEvent.BULLET_SYNC_POS_ROT:
                base.Execute(PlayerEvent.BULLET_SYNC_POS_ROT,message);//调用父类方法
                break;
            case PlayerEvent.PLAYER_SYNC_LIFE:
                base.Execute(PlayerEvent.PLAYER_SYNC_LIFE, message);//调用父类方法
                break;
            case PlayerEvent.PLAYER_DESTROY:
                DestroyPlayer(message);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitGameData()
    {
        //1、找寻坦克需要初始化的位置
        playerPos = GameObject.FindGameObjectsWithTag("StartPos");
        //2、将四坦克全部按位置实例化出来
        for (int i = 0; i < players.Length; i++)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(players[i], GetRealPos(players[i].name), Quaternion.identity);
            onlinePlayers[i] = temp;
        }

        //3、设置本地玩家
        SetLocalPlayer();
    }

    /// <summary>
    /// 设置本地玩家相关信息
    /// </summary>
    private void SetLocalPlayer()
    {
        GameObject tempPlayer = GameObject.Find(playerNames[color - 1]);
        tempPlayer.AddComponent<PlayerControllers>();
        tempPlayer.AddComponent<PlayerAsync>();
        tempPlayer.AddComponent<PlayerAttack>();
        tempPlayer.AddComponent<PlayerHealth>();
        tempPlayer.transform.Find("UserName").GetComponentInChildren<Text>().text = PlayerPrefs.GetString("username");
    }

    private Vector3 GetRealPos(string playerName)
    {
        for (int i = 0; i < playerPos.Length; i++)
        {
            if (!playerName.StartsWith(playerPos[i].name))
            {
                continue;
            }
            return playerPos[i].transform.position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 同步玩家信息到本地
    /// </summary>
    /// <param name="value"></param>
    void SyncPositionAndRotation_SREC(object value)
    {
        playerDto_SREC = value as PlayerDto;
        int identify = playerDto_SREC.color;
        string colors = playerNames[identify - 1];

        for (int i = 0; i < onlinePlayers.Length; i++)
        {
            if (onlinePlayers[i].name.Equals(colors))
                SetRemotePlayerPosAndRot(onlinePlayers[i]);
        }
    }

    /// <summary>
    /// 设置远程玩家的信息
    /// </summary>
    /// <param name="remotePlayer"></param>
    void SetRemotePlayerPosAndRot(GameObject remotePlayer)
    {
        remotePlayer.transform.position = new Vector3(playerDto_SREC.posX, playerDto_SREC.posY, playerDto_SREC.posZ);
        remotePlayer.transform.eulerAngles = new Vector3(playerDto_SREC.rotX, playerDto_SREC.rotY, playerDto_SREC.rotZ);
    }

    void DestroyPlayer(object value)
    {
        PlayerDto playerDto = value as PlayerDto;
        GameObject tempPlayer = GameObject.Find(playerNames[playerDto.color - 1]);
        //1、发送玩家击败的消息
        promptMsg.Change(tempPlayer.transform.Find("UserName").GetComponentInChildren<Text>().text + "已经被击败了", Color.green);
        MsgCenter.Instance.Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        //2、控制玩家的坦克消失
        Destroy(tempPlayer);

    }
}
