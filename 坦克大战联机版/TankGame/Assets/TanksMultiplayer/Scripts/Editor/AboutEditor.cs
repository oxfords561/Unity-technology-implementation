/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace TanksMP
{
    //our about/help/support editor window
    public class AboutEditor : EditorWindow
    {
        [MenuItem("Window/Tanks Multiplayer/About")]
        static void Init()
        {
            AboutEditor aboutWindow = (AboutEditor)EditorWindow.GetWindowWithRect
                    (typeof(AboutEditor), new Rect(0, 0, 300, 300), false, "About");
            aboutWindow.Show();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect((Screen.width / 2) - 60, 20, 250, 100));
            GUILayout.Label("Tanks Multiplayer");
            GUILayout.Label("by Rebound Games");
            GUILayout.EndArea();
            GUILayout.Space(70);

            GUILayout.Label("Info", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Homepage");
            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("www.rebound-games.com");
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("YouTube");
            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("https://www.youtube.com/user/ReboundGamesTV");
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Twitter");
            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("https://twitter.com/Rebound_G");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

        
            GUILayout.Label("Support", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Script Reference");
            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("www.rebound-games.com/docs/tanksmp/");
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Support Forum");
            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("www.rebound-games.com/forum/");
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Unity Forum");
            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("http://forum.unity3d.com/threads/410465/");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            
            GUILayout.Label("Support us!", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rate/Review");

            if (GUILayout.Button("Visit", GUILayout.Width(100)))
            {
                Help.BrowseURL("https://www.assetstore.unity3d.com/#!/publisher/898");
            }
            GUILayout.EndHorizontal();
        }
    }
}