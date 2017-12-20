using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllers : PlayerBase {

    private Rigidbody mRigidbody;
    private float runSpeed = 5f;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
        isLife = true;
        identifyColor = 0;
        //Bind(UIEvent.LOGIN_PANEL_ACTIVE);
    }

    //public override void Execute(int eventCode, object message)
    //{
    //    switch (eventCode)
    //    {
    //        case UIEvent.LOGIN_PANEL_ACTIVE:
    //            //setPanelActive((bool)message);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void FixedUpdate()
    {
        if(isLife)
            InputController();
    }

    void InputController()
    {
        //坦克向上运动
        if (Input.GetKey(KeyCode.W))
        {
            mRigidbody.MovePosition(transform.position + Vector3.forward * Time.deltaTime * runSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.time * 1f);
        }

        //坦克向下运动
        if (Input.GetKey(KeyCode.S))
        {
            mRigidbody.MovePosition(transform.position + Vector3.back * Time.deltaTime * runSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.time * 1f);
        }

        //坦克向左边运动
        if (Input.GetKey(KeyCode.A))
        {
            mRigidbody.MovePosition(transform.position + Vector3.left * Time.deltaTime * runSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.time * 1f);
        }

        //坦克右边运动
        if (Input.GetKey(KeyCode.D))
        {
            mRigidbody.MovePosition(transform.position + Vector3.right * Time.deltaTime * runSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.time * 1f);
        }
    }
}
