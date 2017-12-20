/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections.Generic;
using UnityEngine;

namespace TanksMP
{
    /// <summary>
    /// PLACEHOLDER IMPLEMENTATION. RUN THE NETWORK SETUP!
    /// </summary>
    public class Pool : MonoBehaviour
    {
        public GameObject prefab;
        public int preLoad = 0;
        public bool limit = false;
        public int maxCount;

        public List<GameObject> active = new List<GameObject>();

        public void Awake() {}
        public GameObject Spawn(Vector3 position, Quaternion rotation) { return null; }
        public void Despawn(GameObject instance) {}
        public void Despawn(GameObject instance, float time) {}
        public void DestroyUnused(bool limitToPreLoad) {}
    }


    [System.Serializable]
    public class PoolTimeObject
    {
        public GameObject instance;
        public float time;
    }
}
