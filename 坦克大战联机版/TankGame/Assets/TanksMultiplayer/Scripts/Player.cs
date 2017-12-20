/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEngine.UI;

namespace TanksMP
{          
    /// <summary>
    /// PLACEHOLDER IMPLEMENTATION. RUN THE NETWORK SETUP!
    /// </summary>
	public class Player : MonoBehaviour
    {
		[HideInInspector]
        public string myName;
        public Text label;

		[HideInInspector]
        public int teamIndex;
        public int health = 10;

		[HideInInspector]
        public int maxHealth;
        public int shield = 0;

        [HideInInspector]
        public int turretRotation;

		[HideInInspector]
        public int ammo = 0;

		[HideInInspector]
        public int currentBullet = 0;
        public float fireRate = 0.75f;
        public float moveSpeed = 8f;
        public Slider healthSlider;
        public Slider shieldSlider;
        public AudioClip shotClip;
        public AudioClip explosionClip;
        public GameObject shotFX;
        public GameObject explosionFX;
        public Transform turret;
        public Transform shotPos;
        public GameObject[] bullets;
        public MeshRenderer[] renderers;

        [HideInInspector]
        public GameObject killedBy;
        
        [HideInInspector]
        public FollowTarget camFollow;
    }
}