using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private SticktController sticktController;
    private Rigidbody mRigibody;
    private float moveSpeed = 3f;
    private float offsetX;
    private float offsetY;

	void Start () {
        mRigibody = GetComponent<Rigidbody>();
        sticktController = GameObject.Find("Canvas").transform.Find("JoyStick").GetComponent<SticktController>();
        if(sticktController != null)
            sticktController.OnMoveStickt += Move;
    }
	
    void Move(float offsetX, float offsetY)
    {
        if (offsetY != 0 || offsetX != 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(new Vector3(offsetX, 0, offsetY), Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, newDirection, Time.deltaTime * 10f);

            mRigibody.MovePosition(transform.localPosition + (Vector3.forward * offsetY + (Vector3.right * offsetX)).normalized * moveSpeed * Time.deltaTime);

            
        }
    }
   
}
