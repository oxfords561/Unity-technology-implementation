using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace TanksMP
{
    public class PluginSetup : EditorWindow
    {
        private static string packagesPath;
        private bool shouldReload = false;

        private Packages selectedPackage = Packages.UnityNetworking;
        private enum Packages
        {
            UnityNetworking,
            PhotonPUN
        }


        [MenuItem("Window/Tanks Multiplayer/Network Setup")]
        static void Init()
        {
            packagesPath = "/Packages/";
            EditorWindow window = EditorWindow.GetWindowWithRect(typeof(PluginSetup), new Rect(0, 0, 360, 250), false, "Network Setup");

            var script = MonoScript.FromScriptableObject(window);
            string thisPath = AssetDatabase.GetAssetPath(script);
            packagesPath = thisPath.Replace("/PluginSetup.cs", packagesPath);
        }


        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tanks Multiplayer - Network Setup", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Please choose the network provider you would like to use:");

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            selectedPackage = (Packages)EditorGUILayout.EnumPopup(selectedPackage);

            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                switch (selectedPackage)
                {
                    case Packages.UnityNetworking:
                        Application.OpenURL("https://unity3d.com/services/multiplayer");
                        break;
                    case Packages.PhotonPUN:
                        Application.OpenURL("https://www.photonengine.com/en/Realtime");
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Import"))
            {                		
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                AssetDatabase.ImportPackage(packagesPath + selectedPackage.ToString() + ".unitypackage", false);

				//automatic assignment of PhotonView IDs in non-loaded scenes
                if(selectedPackage == Packages.UnityNetworking)
                    this.Close();
                else
                    shouldReload = true;

                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Note:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("For a detailed comparison about features and pricing, please");
            EditorGUILayout.LabelField("refer to the official pages for UNET or Photon. The features");
            EditorGUILayout.LabelField("of this asset are the same across both multiplayer services.");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Please read the PDF documentation for further details.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Support links: Window > Tanks Multiplayer > About.");

            if (shouldReload)
            {
                this.Focus();
                if (EditorApplication.isCompiling)
                {
                    EditorUtility.DisplayProgressBar("Reloading scenes...", "Wait for the compiler to finish...", 0.8f);
                    return;
                }

                EditorUtility.ClearProgressBar();
                shouldReload = false;

                string[] scenes = System.IO.Directory.GetFiles(".", "*.unity", System.IO.SearchOption.AllDirectories);
                foreach (string scene in scenes)
                {
                    if(scene.Contains("Game.unity"))
                    {
                        EditorSceneManager.OpenScene(scene);
                        //we have to disconnect all prefab connections first, because otherwise they can't be manipulated
                        //in the PhotonViewHandler.HierarchyChange method - seems like a Unity bug
                        ObjectSpawner[] objects = FindObjectsOfType(typeof(ObjectSpawner)) as ObjectSpawner[];
                        if (objects == null || objects.Length == 0) continue;

                        for (int i = 0; i < objects.Length; i++)
                            PrefabUtility.DisconnectPrefabInstance(objects[i].gameObject);

                        EditorApplication.hierarchyWindowChanged();

                        for (int i = 0; i < objects.Length; i++)
                            PrefabUtility.ReconnectToLastPrefab(objects[i].gameObject);

                        //modifying a prefab obviously doesn't mark it as dirty...
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                        //again, for Photon callback
                        EditorApplication.hierarchyWindowChanged();
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scene, false);
                        break;
                    }
                }

                //load intro scene after modifying game scenes
                foreach (string scene in scenes)
                {
                    if (scene.Contains("Intro.unity"))
                    {
                        EditorSceneManager.OpenScene(scene);
                        break;
                    }
                }
            }
        }
    }
}