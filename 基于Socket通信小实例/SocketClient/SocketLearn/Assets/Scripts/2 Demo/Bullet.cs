using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform currentPlayerTransform;
    private ClientManager clientManager;

	void Start () {
        Destroy(gameObject,5f);
	}
	
	void Update () {
		
	}

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            coll.gameObject.GetComponent<PlayerMove>().SendDestroyInfo(coll.gameObject.name);
            Destroy(coll.gameObject);
        }
    }

}
