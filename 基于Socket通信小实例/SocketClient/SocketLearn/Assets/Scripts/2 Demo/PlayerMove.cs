using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [HideInInspector]
    public string flag;
    private BulletRequest bulletRequest;
    public GameObject redBulletPrefab;
    public GameObject greenBulletPrefab;
    private string tempFlag = "";
    private ClientManager clientManager;
    private Rigidbody myRigibody;
    private Vector3 playerInput;
    private float movementSpeed = 5.0f;
    private float turnSpeed = 1000f;
    //同步频率
    private int syncRate = 20;
    private bool isRemotePlayer = false;
    private Vector3 pos;
    private Vector3 rotation;
    private Transform currentPlayerTransform;
    private Transform bulletTransform;

    void Start()
    {

        myRigibody = GetComponent<Rigidbody>();

        clientManager = GameObject.Find("ClientManager").GetComponent<ClientManager>();

        bulletRequest = GetComponent<BulletRequest>();

        bulletRequest.SetClientManager(clientManager);

        currentPlayerTransform = clientManager.currentPlayer.transform;

        if (currentPlayerTransform != null)
            bulletTransform = currentPlayerTransform.Find("BulletPos");

        //持续间隔对数据的同步
        InvokeRepeating("SyncPlayerTransform", 3f, 1f / syncRate);

        InvokeRepeating("SyncOtherPlayerTransform", 3f, 1f / syncRate);
    }

    void SyncOtherPlayerTransform()
    {
        //判断当前客户端控制的对象来同步其他客户端的对象信息
        if (clientManager.currentPlayer != null && clientManager.currentPlayer.name.Equals("OtherPlayer(Clone)"))
        {
            GameObject player= GameObject.Find("Player(Clone)");
            if (player != null)
                SyncRemoteTransform(player.transform);
        }
        else
        {
            GameObject otherPlayer = GameObject.Find("OtherPlayer(Clone)");
            if(otherPlayer != null)
                SyncRemoteTransform(otherPlayer.transform);
        }
    }

    //同步自身控制对象的位置信息给服务器
    void SyncPlayerTransform()
    {
        if (currentPlayerTransform == null) return;

        string data = string.Format("{0},{1},{2}*{3},{4},{5}*{6}", currentPlayerTransform.localPosition.x,
            currentPlayerTransform.localPosition.y, currentPlayerTransform.localPosition.z, currentPlayerTransform.localEulerAngles.x,
            currentPlayerTransform.localEulerAngles.y, currentPlayerTransform.localEulerAngles.z, flag);

        clientManager.SendMsg(data);
    }

    //同步其他客户端的控制对象位置信息
    void SyncRemoteTransform(Transform remotePlayerTransform)
    {
        if (Vector3.Distance(Vector3.zero, pos) == 0) return;
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rotation;
    }

    public void RecieveData(string data)
    {
        if (clientManager.currentPlayer != this.gameObject) return;

        //对位置同步消息的接收
        if (!string.IsNullOrEmpty(data) && !data.Equals("") && data.Contains("*"))
        {
            //获取到其他控制器的数据信息并进行解析
            string[] strs = data.Split('*');
            pos = UnityTools.ParseVector3(strs[0]);
            rotation = UnityTools.ParseVector3(strs[1]);
            tempFlag = strs[2];
        }
        else if(data.Contains("|"))//对子弹生成的消息进行接收
        {
            bulletRequest.HandleResopnse(data);
        }
        else//对角色死亡消息进行接收
        {
            Debug.Log("接收死亡消息");
            RemoteDestroyPlayer(data);
        }

    }

    private void Update()
    {
        //开始发射子弹
        Shoot();
    }

    void Shoot()
    {
        if (clientManager.currentPlayer != this.gameObject) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet;
            if (clientManager.currentPlayer.name.Equals("Player(Clone)"))
            {
                bullet = GameObject.Instantiate<GameObject>(redBulletPrefab,bulletTransform.position,Quaternion.identity);
                bulletRequest.SendRequest(redBulletPrefab.name, bulletTransform.position, redBulletPrefab.transform.eulerAngles);
            }
            else
            {
                bullet = GameObject.Instantiate<GameObject>(greenBulletPrefab, bulletTransform.position, Quaternion.identity);
                bulletRequest.SendRequest(greenBulletPrefab.name, bulletTransform.position, redBulletPrefab.transform.eulerAngles);
            }

            bullet.GetComponent<Rigidbody>().AddForce(bulletTransform.forward * 2000);
        }
    }

    void FixedUpdate()
    {
        //如果当前控制对象不属于此客户端则不控制
        if (clientManager.currentPlayer != this.gameObject)
        {
            return;
        }

        //设置 坐标的值
        playerInput.Set(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //当用户停止输入 让当前的物体不做任何的操作且待在原地
        if (playerInput == Vector3.zero)
            return;

        //创建一个可以朝向运动的方向
        Quaternion newRotation = Quaternion.LookRotation(playerInput);


        //如果当前物体并没有朝向运动方向则将当前物体转向运动方向
        if (myRigibody.rotation != newRotation)
            myRigibody.rotation = Quaternion.RotateTowards(myRigibody.rotation, newRotation, turnSpeed * Time.deltaTime);

        //通过我们的输入得到单位的方向向量，再乘以移动速度以及时间得到需要移动的距离Vector3（此时是以原点为基准的）
        //再和此前物体的坐标相加可以得到当前物体需要移动到的新坐标
        Vector3 newPosition = transform.localPosition + playerInput.normalized * movementSpeed * Time.deltaTime;

        //将我们的物体移动到最新的坐标
        myRigibody.MovePosition(newPosition);
    }

    //同步远程客户端创建子弹
    public void RemoteCreateBullet(string bulletName,Vector3 position,Vector3 rotation)
    {
        GameObject bulletPrefab = Resources.Load<GameObject>(bulletName);
        GameObject bullet = GameObject.Instantiate<GameObject>(bulletPrefab);
        bullet.transform.position = position;
        bullet.transform.eulerAngles = rotation;

        bullet.GetComponent<Rigidbody>().AddForce(bulletTransform.forward * 2000);
    }

    //发送死亡消息
    public void SendDestroyInfo(string destroyName)
    {
        clientManager.SendMsg(destroyName);
    }

    public void RemoteDestroyPlayer(string playerName)
    {
        Debug.Log("更新远程死亡消息");
        Destroy(GameObject.Find(playerName));
    }

}
