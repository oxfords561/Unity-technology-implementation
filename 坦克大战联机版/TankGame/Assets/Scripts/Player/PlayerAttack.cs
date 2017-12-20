using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 针对坦克进行炮弹攻击
/// </summary>
public class PlayerAttack : PlayerBase {

    private GameObject bullet_CREQ;
    private GameObject bullet_SREC;
    private string[] bulletNames = new string[4] { "RedShell", "YellowShell", "GreenShell", "BlueShell" };
    private string[] playerNames = new string[4] { "RedTank(Clone)", "YellowTank(Clone)", "GreenTank(Clone)", "BlueTank(Clone)" };
    private GameObject bulletPos;
    private float power = 3000f;
    private SocketMsg socketMsg = new SocketMsg();
    private BulletDto bulletDto_CREQ = new BulletDto();
    private BulletDto bulletDto_SREC = new BulletDto();
    private Vector3 bulletSyncPos = Vector3.zero;
    private Vector3 bulletSyncRot = Vector3.zero;

    void Start()
    {
        Bind(PlayerEvent.BULLET_SYNC_POS_ROT);
        bulletPos = transform.Find("BulletPos").gameObject;
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case PlayerEvent.BULLET_SYNC_POS_ROT:
                SetRemoteBulletPosAndRot(message);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (bullet_CREQ == null && identifyColor != 0)
            bullet_CREQ = Resources.Load<GameObject>(bulletNames[identifyColor - 1]);

        //鼠标左键按下开始发射炮弹
        if (Input.GetMouseButtonDown(0) && bullet_CREQ != null)
        {
            GameObject bulletObject = GetBullet();

            //将子弹同步到服务器
            bulletDto_CREQ.color = identifyColor;
            bulletDto_CREQ.posX = bulletObject.transform.position.x;
            bulletDto_CREQ.posY = bulletObject.transform.position.y;
            bulletDto_CREQ.posZ = bulletObject.transform.position.z;
            bulletDto_CREQ.rotX = bulletObject.transform.eulerAngles.x;
            bulletDto_CREQ.rotY = bulletObject.transform.eulerAngles.y;
            bulletDto_CREQ.rotZ = bulletObject.transform.eulerAngles.z;
            socketMsg.Change(OpCode.Player, PlayerCode.BULLET_SYNC_POS_ROT_CREQ, bulletDto_CREQ);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
    }

    private GameObject GetBullet()
    {
        //初始化子弹
        GameObject bulletObject = Instantiate<GameObject>(bullet_CREQ, bulletPos.transform.position, transform.rotation);
        bulletObject.GetComponent<Rigidbody>().AddForce(transform.forward * power);
        return bulletObject;
    }

    /// <summary>
    /// 设置远程子弹
    /// </summary>
    /// <param name="value"></param>
    void SetRemoteBulletPosAndRot(object value)
    {
        bulletDto_SREC = value as BulletDto;
        int bulletColor = bulletDto_SREC.color;
        bulletSyncPos = new Vector3(bulletDto_SREC.posX,bulletDto_SREC.posY,bulletDto_SREC.posZ);
        bulletSyncRot = new Vector3(bulletDto_SREC.rotX,bulletDto_SREC.rotY,bulletDto_SREC.rotZ);

        GameObject tempPlayer = GameObject.Find(playerNames[bulletColor - 1]);
        bullet_SREC = Resources.Load<GameObject>(bulletNames[bulletColor - 1]);
        GameObject bulletObj = Instantiate<GameObject>(bullet_SREC, bulletPos.transform.position, tempPlayer.transform.rotation); //需要优化
        bulletObj.transform.position = bulletSyncPos;
        bulletObj.transform.eulerAngles = bulletSyncRot;
        bulletObj.GetComponent<Rigidbody>().AddForce(tempPlayer.transform.forward * power);
    }

}
