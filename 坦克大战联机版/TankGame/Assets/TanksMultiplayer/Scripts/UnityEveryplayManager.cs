/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

#if EVERYPLAY_IPHONE || EVERYPLAY_TVOS || EVERYPLAY_ANDROID || EVERYPLAY_STANDALONE
#define UNITY_EVERYPLAY
#endif
 
using System;
using UnityEngine;
using UnityEngine.UI;

namespace TanksMP
{
    /// <summary>
    /// Manager handling the full workflow of recording gameplay videos,
    /// creating thumbnails for them and offering sharing dialogs using Unity Everyplay.
    /// </summary>
    public class UnityEveryplayManager : MonoBehaviour
    {      
        #pragma warning disable 0067
        /// <summary>
        /// Fired on SDK initialization to deliver supported recording state.
        /// </summary>
        public static event Action<bool> recordingSupportedEvent;
      	
        //reference to the thumbnail texture being used for display
		private static Texture2D thumbnailTexture;
        #pragma warning restore 0067

		
		#if UNITY_EVERYPLAY	
        //add callback listeners
        void Start()
        {
			Everyplay.ReadyForRecording += OnReadyForRecording;
			Everyplay.UploadDidComplete += OnShareComplete;
        }

      
        //called by Everplay when its internal initialization completes
        void OnReadyForRecording(bool enabled)
        {
            if(recordingSupportedEvent != null)
                recordingSupportedEvent(enabled);
        }
		#endif
      
		
		/// <summary>
		/// Returns whether Everplay is supported on the device.
		/// </summary>
		public static bool IsSupported()
		{
			bool supported = false;
			#if UNITY_EVERYPLAY
			supported = Everyplay.IsSupported();
			#endif
			
			return supported;
		}
		
		
		/// <summary>
		/// Returns whether Everplay recording is supported on the device.
		/// </summary>
		public static bool IsRecordingSupported()
		{
			bool supported = false;
			#if UNITY_EVERYPLAY
			supported = Everyplay.IsRecordingSupported();
			#endif
			
			return supported;
		}
	  
	  
        /// <summary>
        /// Starts recording gameplay videos, if recording is supported for this device
        /// (determined by Everyplay) and the user enabled screen recording in the game settings.
        /// </summary>
        public static void StartRecord()
        {
			#if UNITY_EVERYPLAY	
            //don't record with recording turned off
            if(!bool.Parse(PlayerPrefs.GetString(PrefsKeys.recordGame)))
				return;

            //start recording if supported and ready
            if(Everyplay.IsRecordingSupported() && Everyplay.IsReadyForRecording())
				Everyplay.StartRecording();
			#endif
        }
      
      
        /// <summary>
        /// Pauses the recording of gameplay videos, if running.
        /// </summary>
        public static void PauseRecord()
        {
			#if UNITY_EVERYPLAY	
            if(Everyplay.IsRecording())
                Everyplay.PauseRecording();
			#endif
        }
      
      
        /// <summary>
        /// Resumes the recording of gameplay videos, if paused.
        /// </summary>
        public static void ResumeRecord()
        {
			#if UNITY_EVERYPLAY	
            if(Everyplay.IsPaused())
                Everyplay.ResumeRecording();
			#endif
        }
      
        
        /// <summary>
        /// Stops the recording of gameplay videos, if running or paused.
        /// </summary>
        public static void StopRecord()
		{
			#if UNITY_EVERYPLAY	
            if(Everyplay.IsRecording() || Everyplay.IsPaused())
                Everyplay.StopRecording();
			#endif
        }


        /// <summary>
        /// Initializes the thumbnail texture while recording with a valid target,
        /// setting its texture sizes matching the target and declares it with Everplay.
        /// </summary>
		public static void InitializeThumbnail(Image target)
		{
			#if UNITY_EVERYPLAY	
            //get width and height of target
			int thumbnailWidth = (int)target.rectTransform.rect.width;
			int thumbnailHeight = (int)target.rectTransform.rect.height;

			//create a texture for the thumbnail with target size
			thumbnailTexture = new Texture2D (thumbnailWidth, thumbnailHeight, TextureFormat.RGBA32, false);
			thumbnailTexture.wrapMode = TextureWrapMode.Clamp;

			//set texture target in Everplay
			Everyplay.SetThumbnailTargetTexture(thumbnailTexture);

			//create new sprite out of texture
			target.sprite = Sprite.Create (thumbnailTexture, new Rect (0, 0, thumbnailWidth, thumbnailHeight), Vector2.zero);
            target.color = Color.white;
			#endif
		}
      
      
        /// <summary>
        /// Takes a thumbnail of the current gameplay frame, while recording.
        /// </summary>
        public static void TakeThumbnail()
        {
			#if UNITY_EVERYPLAY	
            Everyplay.TakeThumbnail();
			#endif
        }
      
      
        /// <summary>
        /// Shows the Everyplay community hub.
        /// </summary>
        public static void ShowMenu()
        {
			#if UNITY_EVERYPLAY	
            Everyplay.Show();
			#endif
        }
      
      
        /// <summary>
        /// Shows the Everyplay sharing dialog.
        /// </summary>
        public static void Share()
        {
            //UnityAnalyticsManager.ShareStart();
			
			#if UNITY_EVERYPLAY	
            Everyplay.ShowSharingModal();
			#endif
        }

		
        //track successfully shared videos in Unity Analytics
        //---not implemented yet---
        void OnShareComplete(int videoId)
        {
            //UnityAnalyticsManager.ShareComplete();
        }
    }
}
