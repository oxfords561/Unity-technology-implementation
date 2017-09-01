using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform currentPlayerTransform;
    private ClientManager clientManager;

	void Start () {
        Destroy(gameObject,5f);
        //if()
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //同步自身控制对象的位置信息给服务器
    void SyncPlayerTransform()
    {
        //string data = string.Format("{0},{1},{2}*{3},{4},{5}*{6}", currentPlayerTransform.localPosition.x,
        //    currentPlayerTransform.localPosition.y, currentPlayerTransform.localPosition.z, currentPlayerTransform.localEulerAngles.x,
        //    currentPlayerTransform.localEulerAngles.y, currentPlayerTransform.localEulerAngles.z, flag);

        //clientManager.SendMsg(data);
    }

    //同步其他客户端的控制对象位置信息
    void SyncRemoteTransform(Transform remotePlayerTransform)
    {
        //if (Vector3.Distance(Vector3.zero, pos) == 0) return;
        //remotePlayerTransform.position = pos;
        //remotePlayerTransform.eulerAngles = rotation;
    }


}
