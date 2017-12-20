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
    public class UIMain : MonoBehaviour
    {
        public GameObject loadingWindow;
        public GameObject connectionErrorWindow;
        public GameObject billingErrorWindow;
		public Button everyplayButton;
		public InputField nameField;
        public Dropdown networkDrop;
		public InputField serverField;
        public Toggle musicToggle;
		public Slider volumeSlider;
        public Toggle recordToggle;
       
        public void Play()
        {
            Debug.LogError("PLEASE READ THE README! You can't play the game without following the instructions.");
        }
        
        public void OpenShop() {}
        public void OpenSettings() {}
		public void OnNetworkChanged(int value) {}
        public void OnMusicChanged(bool value) {}
        public void OnVolumeChanged(float value) {}
        public void CloseSettings() {}
		public void OnEveryplaySupport(bool supported) {}
		public void ShowEveryplay() {}
        public void RateApp() {}
    }
}