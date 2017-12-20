/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace TanksMP
{
    /// <summary>
    /// Manager handling the full workflow of showing video ads, reacting
    /// to completed/failed video views and resulting events. Implements a
    /// custom, percentage based chance for showing ads. Using Unity Ads.
    /// </summary>
    public class UnityAdsManager : MonoBehaviour
    {
        #if UNITY_ADS
        /// <summary>
        /// Fired whenever a view completes, providing the result.
        /// </summary>
        public static event Action<ShowResult> adResultEvent;
        
        //reference to this script instance
        private static UnityAdsManager instance;
        
        //counter of ad display attempts
        private static int counter = 0;
        
        //whether an ad has been shown during one game round
        private static bool adShown = false;
        
        
        /// <summary>
        /// Returns a reference to this script instance.
        /// </summary>
        public static UnityAdsManager GetInstance()
        {
            return instance;
        }


        //sets the instance reference, if not set already,
        //and keeps listening to scene changes.
        void Awake()
        {
            if (instance != null)
                return;

            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        
        //reset ad states on scene switch
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            counter = 0;
            adShown = false;
        }
            
        
        /// <summary>
        /// Tries to display a video ad. This could fail if an ad is not available or ready,
        /// the player requesting the ad is hosting the game - in which case showing an ad would
        /// pause the game for all clients - an ad was already shown or the percentage based
        /// chance has calculated to not show an ad for this attempt. All these checks can be
        /// skipped by setting the argument passed in to true, effectively forcing an ad to show.
        /// </summary>
        public static bool ShowAd(bool force = false)
        {
			if(force || !GameManager.isMaster() && !adShown && GetInstance().shouldShowAd())
            {
                if(Advertisement.IsReady())
                {
                    //this attempt should show an ad:  initialize ad and pass the result to
                    //the HandleResult method. Stop recording gameplay video while showing an ad
                    Advertisement.Show("", new ShowOptions {resultCallback = GetInstance().HandleResult});
                    UnityEveryplayManager.PauseRecord();
                    return true;
                }
            }
            
            //at this point we were not able to show an ad yet.
            //if this ad attempt has not been forced, increase attempt counter
            //so the next attempt has a higher probability of showing an ad
            if(!force) counter++;
                
            return false;
        }
        
        
        /// <summary>
        /// Returns whether an ad has been shown already.
        /// </summary>
        public static bool didShowAd()
        {
            return adShown;
        }


        //called by Unity Ads on completed video views
        private void HandleResult(ShowResult result)
        {
            //if the result is finished or skipped,
            //the ad was actually shown during the game
            switch(result)
            {
              case ShowResult.Finished:
              case ShowResult.Skipped:
                  adShown = true;
                  break;
            }
            
            //pass result to listeners
            if(adResultEvent != null)
                adResultEvent(result);
                
            //resume gameplay recording after displaying ad
            UnityEveryplayManager.ResumeRecord();
        }
        
        
        //calculates the chance for showing an ad,
        //based on the total attempt count
        private bool shouldShowAd()
        {   
            //multiply chance by attempt count in steps of 20%
            //example: 1. attempt = 0%, 2.= 20%, 3.= 40%, 4.= 60%, 5.= 80%, 6.= 100%
            //we should then show an ad if the random value then falls into this range
            //this is called on player death, meaning after 6 deaths the chance is 100%
            float adChance = Mathf.Clamp01(counter * 0.2f);
            float adValue = UnityEngine.Random.value;
            bool adTrigger = adValue <= adChance;
            
            //Debug.Log("Chance: " + adChance + ", Random: " + adValue + " -> " + adTrigger);
            return adTrigger;
        }
        #endif
    }
}
