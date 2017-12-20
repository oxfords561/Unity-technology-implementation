/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TanksMP
{
    /// <summary>
    /// Joystick component for controlling player movement and actions using Unity UI events.
    /// There can be multiple joysticks on the screen at the same time, implementing different callbacks.
    /// </summary>
    public class UIJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// Callback triggered when joystick starts moving by user input.
        /// </summary>
        public event Action onDragBegin;
        
        /// <summary>
        /// Callback triggered when joystick is moving or hold down.
        /// </summary>
        public event Action<Vector2> onDrag;
        
        /// <summary>
        /// Callback triggered when joystick input is being released.
        /// </summary>
        public event Action onDragEnd;
       
        /// <summary>
        /// The target object i.e. jostick thumb being dragged by the user.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Maximum radius for the target object to be moved in distance from the center.
        /// </summary>
        public float radius = 50f;
        
        /// <summary>
        /// Current position of the target object on the x and y axis in 2D space.
        /// Values are calculated in the range of [-1, 1] translated to left/down right/up.
        /// </summary>
        public Vector2 position;
        
        //keeping track of current drag state
        private bool isDragging = false;
        
        //reference to thumb being dragged around
		private RectTransform thumb;


        //initialize variables
		void Start()
		{
			thumb = target.GetComponent<RectTransform>();

			//in the editor, disable input received by joystick graphics:
            //we want them to be visible but not receive or block any input
			#if UNITY_EDITOR
				Graphic[] graphics = GetComponentsInChildren<Graphic>();
				for(int i = 0; i < graphics.Length; i++)
					graphics[i].raycastTarget = false;
			#endif
		}


        /// <summary>
        /// Event fired by UI Eventsystem on drag start.
        /// </summary>
        public void OnBeginDrag(PointerEventData data)
        {
            isDragging = true;
            if(onDragBegin != null)
                onDragBegin();
        }


        /// <summary>
        /// Event fired by UI Eventsystem on drag.
        /// </summary>
        public void OnDrag(PointerEventData data)
        {
            //get RectTransforms of involved components
            RectTransform draggingPlane = transform as RectTransform;
            Vector3 mousePos;

            //check whether the dragged position is inside the dragging rect,
            //then set global mouse position and assign it to the joystick thumb
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, data.position, data.pressEventCamera, out mousePos))
            {
                thumb.position = mousePos;
            }

            //length of the touch vector (magnitude)
            //calculated from the relative position of the joystick thumb
            float length = target.localPosition.magnitude;

            //if the thumb leaves the joystick's boundaries,
            //clamp it to the max radius
            if (length > radius)
            {
                target.localPosition = Vector3.ClampMagnitude(target.localPosition, radius);
            }

            //set the Vector2 thumb position based on the actual sprite position
            position = target.localPosition;
            //smoothly lerps the Vector2 thumb position based on the old positions
            position = position / radius * Mathf.InverseLerp(radius, 2, 1);
        }
        
        
        //set joystick thumb position to drag position each frame
        void Update()
        {
            //in the editor the joystick position does not move, we have to simulate it
			//mirror player input to joystick position and calculate thumb position from that
			#if UNITY_EDITOR
				target.localPosition =  position * radius;
				target.localPosition = Vector3.ClampMagnitude(target.localPosition, radius);
			#endif

            //check for actual drag state and fire callback. We are doing this in Update(),
            //not OnDrag, because OnDrag is only called when the joystick is moving. But we
            //actually want to keep moving the player even though the jostick is being hold down
            if(isDragging && onDrag != null)
                onDrag(position);
        }


        /// <summary>
        /// Event fired by UI Eventsystem on drag end.
        /// </summary>
        public void OnEndDrag(PointerEventData data)
        {
            //we aren't dragging anymore, reset to default position
            position = Vector2.zero;
            target.position = transform.position;
            
            //set dragging to false and fire callback
            isDragging = false;
            if (onDragEnd != null)
                onDragEnd();
        }
    }
}