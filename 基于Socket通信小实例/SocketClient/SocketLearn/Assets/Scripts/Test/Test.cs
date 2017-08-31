using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject bullet;
    private Transform bulletPos;

	// Use this for initialization
	void Start () {
        bulletPos = transform.Find("BulletPos");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = Instantiate<GameObject>(bullet, bulletPos.position,Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(bulletPos.forward*8000);
        }
	}
}
