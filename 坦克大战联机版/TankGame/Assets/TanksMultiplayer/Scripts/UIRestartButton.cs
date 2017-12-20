/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TanksMP
{
    /// <summary>
    /// This script is attached to a runtime-generated gameobject in the game scene,
    /// taken over to the intro scene to directly request starting a new multiplayer game.
    /// </summary>
    public class UIRestartButton : MonoBehaviour 
    {
        //listen to scene changes
        void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        
        //give the scene some time to initialize
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Invoke("EnterPlay", 0.5f);
        }
        
        
        //call the play button instantly on scene load
        //destroy itself after use
        void EnterPlay()
        {
            FindObjectOfType<UIMain>().Play();
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}
