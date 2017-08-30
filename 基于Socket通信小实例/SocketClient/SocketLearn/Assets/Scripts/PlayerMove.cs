using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [HideInInspector]
    public string flag;
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

    void Start () {

        myRigibody = GetComponent<Rigidbody>();

        clientManager = GameObject.Find("ClientManager").GetComponent<ClientManager>();

        currentPlayerTransform = clientManager.currentPlayer.transform;

        //持续间隔对数据的同步
        InvokeRepeating("SyncPlayerTransform", 3f, 1f / syncRate);

        InvokeRepeating("SyncOtherPlayerTransform", 3f, 1f / syncRate);
    }

    void SyncOtherPlayerTransform()
    {
        //判断当前客户端控制的对象来同步其他客户端的对象信息
        if (clientManager.currentPlayer.name.Equals("OtherPlayer(Clone)"))
        {
            SyncRemoteTransform(GameObject.Find("Player(Clone)").transform);
        }
        else
        {
            SyncRemoteTransform(GameObject.Find("OtherPlayer(Clone)").transform);
        }
    }

    //同步自身控制对象的位置信息给服务器
    void SyncPlayerTransform()
    {
        string data = string.Format("{0},{1},{2}*{3},{4},{5}*{6}", currentPlayerTransform.localPosition.x,
            currentPlayerTransform.localPosition.y, currentPlayerTransform.localPosition.z,currentPlayerTransform.localEulerAngles.x,
            currentPlayerTransform.localEulerAngles.y,currentPlayerTransform.localEulerAngles.z, flag);

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
     
        if (!string.IsNullOrEmpty(data)&& !data.Equals("")){
            //获取到其他控制器的数据信息并进行解析
            string[] strs = data.Split('*');
            pos = Parse(strs[0]);
            rotation = Parse(strs[1]);
            tempFlag = strs[2];
            Debug.Log("pos "+pos);
            Debug.Log("tempFlag "+tempFlag);
        }
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet;
            if (clientManager.currentPlayer.name.Equals("Player(Clone)"))
                bullet = Instantiate<GameObject>(redBulletPrefab, currentPlayerTransform.position, Quaternion.identity);
            else
                bullet = Instantiate<GameObject>(greenBulletPrefab, currentPlayerTransform.position, Quaternion.identity);

            bullet.GetComponent<Rigidbody>().AddForce(currentPlayerTransform.forward * 100);
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

    //对字符串数据进行解析
     Vector3 Parse(string data)
    {
        string[] strs = data.Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);

        return new Vector3(x,y,z);
    }
}
