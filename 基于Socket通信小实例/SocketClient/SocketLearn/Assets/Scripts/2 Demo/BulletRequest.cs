using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRequest : MonoBehaviour
{

    private ClientManager clientManager;
    private PlayerMove playerMove;
    private string _bulletName;
    private Vector3 _pos;
    private Vector3 _rotation;
    private bool isReturn = false;

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    public void SetClientManager(ClientManager manager)
    {
        this.clientManager = manager;
    }


    private void Update()
    {
        if (isReturn)
        {
            isReturn = false;
            playerMove.RemoteCreateBullet(_bulletName,_pos,_rotation);
        }
    }

    public void SendRequest(string bulletName, Vector3 pos, Vector3 rotation)
    {
        string bulletInfo = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", bulletName, pos.x, pos.y, pos.z, rotation.x, rotation.y, rotation.z);
        clientManager.SendMsg(bulletInfo);
    }

    public void HandleResopnse(string data)
    {
        string[] strs = data.Split('|');
        _bulletName = strs[0];
        _pos = UnityTools.ParseVector3(strs[1]);
        _rotation = UnityTools.ParseVector3(strs[2]);

        isReturn = true;
    }
}
