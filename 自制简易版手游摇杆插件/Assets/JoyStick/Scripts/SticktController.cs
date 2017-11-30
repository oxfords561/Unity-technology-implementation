using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SticktController : MonoBehaviour {

	private RectTransform sticktTransform;
	private RectTransform sticktBgTransform;
	//private GameObject touchRotatePanel;
	private Vector3 firstSticktPos;
	private Vector2 firstSticktPos2;
	private bool isUp = false;
	private bool isDown = false;
	private float backSpeed = 4f;
	private float radius;
	private float offsetX;
	private float offsetY;
	private float rotateX;
	private float rotateY;
	private Vector3 downMousePos;

	public Action<float,float> OnMoveStickt;
	public Action<float,float> OnTouchRotate;
	public Action OnJumpClick;
	public Action OnShootClick;

	void Start () {
		//touchRotatePanel = transform.Find ("TouchRotate").gameObject;
		sticktTransform = transform.Find ("SticktBg/Stickt").GetComponent<RectTransform> ();
		sticktBgTransform = transform.Find ("SticktBg").GetComponent<RectTransform> ();
		firstSticktPos = sticktTransform.position;
		firstSticktPos2 = sticktTransform.anchoredPosition;
		radius =  sticktBgTransform.sizeDelta.x / 2;

	}
	
	void Update () {

		if (isUp) {
			sticktTransform.position = Vector3.Lerp (sticktTransform.position,firstSticktPos,Time.deltaTime * backSpeed);
		}

		if (isDown) {
			if (OnMoveStickt != null) {
				OnMoveStickt (offsetX, offsetY);
			}
		}
	}

	public void OnSticktDrag(){
		
		float distance = Vector3.Distance (sticktBgTransform.position, Input.mousePosition);
		if (distance < radius) {
			sticktTransform.position = Input.mousePosition;

		} else {
			Vector3 targetDirection = Input.mousePosition - firstSticktPos;
			sticktTransform.position = firstSticktPos +  targetDirection.normalized * radius;
			 
		}

		offsetX = sticktTransform.position.x - firstSticktPos.x;
		offsetY = sticktTransform.position.y - firstSticktPos.y;
	}

	public void OnPointerUp(){
		isUp = true;
		isDown = false;
	}

	public void OnPointerDown(){
		isUp = false;
		isDown = true;
	}

	//public void OnTouchDragRotate(){


	//	rotateX = Input.mousePosition.x - downMousePos.x;
	//	rotateY = Input.mousePosition.y - downMousePos.y;

	//	if (OnTouchRotate != null) {
	//		OnTouchRotate (rotateX, rotateY);
	//	}
	//}

	public void OnTouchDragDown(){
		downMousePos = Input.mousePosition;
	}

	public void OnJumpPointerDown(){
		if (OnJumpClick != null) {
			OnJumpClick ();
		}
	}

	public void OnShootPointerDown(){
		if (OnShootClick != null) {
			OnShootClick ();
		}
	}

	public void OnQuitGame(){
		Application.Quit ();
	}
}
