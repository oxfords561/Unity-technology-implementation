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
    public class UIGame : MonoBehaviour
    {
        public UIJoystick[] controls;
        public Slider[] teamSize;
        public Text[] teamScore;
        public GameObject aimIndicator;
        public Text deathText;
        public Text spawnDelayText;
		public Image thumbnailImage;
        public Text gameOverText;
        public GameObject gameOverMenu;

        public void ToggleControls(bool state) {}
        public void SetDeathText(string playerName, Team team) {}
        public void SetSpawnDelay(float time) {}
        public void DisableDeath() {}
        public void SetGameOverText(Team team) {}
        public void ShowGameOver() {}
        public void Restart() {}
        public void Share() {}
        public void Quit() {}
    }
}

