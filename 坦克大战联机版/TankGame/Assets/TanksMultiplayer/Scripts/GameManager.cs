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
	public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public Player localPlayer;
        public UIGame ui;
        public Team[] teams;
        public int maxScore = 30;
        public int respawnTime = 8;

        public static GameManager GetInstance() { return null; }
		public static bool isMaster() { return false; }
        public int GetTeamFill() { return 0; }
        public Vector3 GetSpawnPosition(int teamIndex) { return Vector3.zero; }
        public bool IsGameOver() { return false; }
        public void DisplayDeath(bool skipAd = false) {}
        public void DisplayGameOver(int teamIndex) {}
    }

    [System.Serializable]
    public class Team
    {
        public string name;
        public Material material;
        public Transform spawn;
    }
}