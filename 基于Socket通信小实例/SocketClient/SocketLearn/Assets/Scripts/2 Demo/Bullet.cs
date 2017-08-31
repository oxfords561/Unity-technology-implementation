using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(transform.localPosition);
        Destroy(gameObject,5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
