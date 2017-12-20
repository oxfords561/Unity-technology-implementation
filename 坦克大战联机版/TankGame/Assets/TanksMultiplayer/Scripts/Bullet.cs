/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

namespace TanksMP
{
    /// <summary>
    /// PLACEHOLDER IMPLEMENTATION. RUN THE NETWORK SETUP!
    /// </summary>
	public class Bullet : MonoBehaviour
    {
        public float speed = 18;
        public int damage = 3;
        public float despawnDelay = 1f;
		public int bounce = 0;
        public AudioClip hitClip;
        public AudioClip explosionClip;
        public GameObject hitFX;
        public GameObject explosionFX;

        [HideInInspector]
        public GameObject owner;
    }
}
