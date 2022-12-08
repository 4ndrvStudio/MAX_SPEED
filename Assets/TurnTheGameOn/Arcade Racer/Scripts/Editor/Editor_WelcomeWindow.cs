#if UNITY_EDITOR
namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEditor;
    using UnityEngine;
    public class Editor_WelcomeWindowSetupAfterImport : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {

                if (str.Contains("Editor_WelcomeWindow"))
                {
                    Editor_WelcomeWindow.OnImportCheckShowWelcome();
                }
            }
        }
    }
    [InitializeOnLoad]
    public class Editor_WelcomeWindow : EditorWindow
    {
        public static Editor_WelcomeWindow editorWindow;
        protected static bool launchedWindow = false;
        protected static int windowTextureWidth = 512;
        protected static int windowTextureHeight = 55;
        protected Texture2D windowTexture;

        public static void OnImportCheckShowWelcome()
        {
            EditorApplication.update += CheckShowWelcomeOnImport;
        }

        static Editor_WelcomeWindow()
        {
            EditorApplication.update += CheckShowWelcome;
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Welcome Window")]
        public static void ShowWelcomeScreen()
        {
            if (EditorData.ProjectInfo) EditorData.ProjectInfo.CompletedWelcome();
            float windowWidth = windowTextureWidth + 8;
            float windowHeight = (windowTextureHeight * 0.75f) + 224;
            Rect rect = new Rect((Screen.currentResolution.width - windowWidth) / 2.0f,
                (Screen.currentResolution.height - windowHeight) / 2.0f,
                windowWidth, windowHeight);
            string assetVersion = EditorData.ProjectInfo != null ? " (Version: " + EditorData.ProjectInfo.assetVersion + ")" : "";
            editorWindow = (Editor_WelcomeWindow)EditorWindow.GetWindowWithRect(typeof(Editor_WelcomeWindow), rect, true, "Arcade Racer: Racing Game Development Kit" + assetVersion);
            editorWindow.minSize = new Vector2(windowWidth, windowHeight);
            editorWindow.maxSize = new Vector2(windowWidth, windowHeight);
            editorWindow.position = rect;
            editorWindow.Show();
            launchedWindow = true;
        }

        public static void CheckShowWelcome()
        {
            if (EditorData.ProjectInfo != null)
            {
                EditorApplication.update -= CheckShowWelcome;
                if (Time.realtimeSinceStartup > 3) launchedWindow = true;
                if (!launchedWindow && EditorData.ProjectInfo.useLaunchWindow)
                {
                    if (!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        ShowWelcomeScreen();
                    }
                }
            }
        }

        public static void CheckShowWelcomeOnImport()
        {
            if (EditorData.ProjectInfo != null && EditorPrefs.GetBool("WelcomeWindowWasLaunchedOnImport") == false)
            {
                if (EditorData.ProjectInfo.useLaunchWindow)
                {
                    if (!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        EditorApplication.update -= CheckShowWelcome;
                        EditorApplication.update -= CheckShowWelcomeOnImport;
                        EditorPrefs.SetBool("WelcomeWindowWasLaunchedOnImport", true);
                        ShowWelcomeScreen();
                    }
                }
            }
        }

        void OnGUI()
        {
            if (EditorData.ProjectInfo != null)
            {

                GUIStyle buttonStyle = new GUIStyle("button");
                buttonStyle.fontSize = 11;
                //buttonStyle.fontStyle = FontStyle.BoldAndItalic;


                
                if (windowTexture == null) windowTexture = Resources.Load<Texture2D>("ArcadeRacer_WelcomeHeader");
                GUILayout.Label(windowTexture, GUILayout.Width(windowTextureWidth), GUILayout.Height(windowTextureHeight));


                #region Import Project Settings
                //EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Import Project Settings");
                EditorGUILayout.HelpBox("Importing 'Project Settings' will overwrite your current 'Project Settings'.", MessageType.Warning, true);

                #region Standalone Game Template
                if (GUILayout.Button("Standalone Game Template", buttonStyle, GUILayout.Height(20)))
                {
                    EditorApplication.ExecuteMenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/StandaloneGameTemplate");
                }
                #endregion

                //#region Mobile Game Template
                //if (GUILayout.Button("Mobile Game Template", buttonStyle, GUILayout.Height(20)))
                //{
                //    EditorApplication.ExecuteMenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/MobileGameTemplate");
                //}
                //#endregion

                #region Standalone Game Template EVP
                if (GUILayout.Button("Standalone Game Template (EVP) Requires Edy's Vehicle Physics", buttonStyle, GUILayout.Height(20)))
                {
                    EditorApplication.ExecuteMenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/StandaloneGameTemplate_EVP");
                }
                #endregion

                //#region Mobile Game Template EVP
                //if (GUILayout.Button("Mobile Game Template (EVP) Requires Edy's Vehicle Physics", buttonStyle, GUILayout.Height(20)))
                //{
                //    EditorApplication.ExecuteMenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/StandaloneGameTemplate_EVP");
                //}
                //#endregion

                EditorGUILayout.EndVertical();
                #endregion



                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Quick Start Video Tutorial"))
                {
                    Application.OpenURL("https://youtu.be/M0hbGf-Juxs");
                }

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Discussion Forum", buttonStyle, GUILayout.Height(20)))
                {
                    Application.OpenURL("https://forum.unity.com/threads/arcade-racer-racing-game-development-kit.380924/");
                }

                if (GUILayout.Button("Request Support", buttonStyle, GUILayout.Height(20)))
                {
                    Application.OpenURL("https://www.turnthegameon.com/contact");
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.Label("", GUILayout.Height(1));



                GUILayout.BeginHorizontal();
                GUI.skin.toggle.alignment = TextAnchor.UpperCenter;
                bool showOnStartUp = GUILayout.Toggle(EditorData.ProjectInfo.useLaunchWindow, " Launch this window when Unity is opened?");
                if (EditorData.ProjectInfo.useLaunchWindow != showOnStartUp)
                {
                    EditorData.ProjectInfo.useLaunchWindow = showOnStartUp;
                    EditorUtility.SetDirty(EditorData.ProjectInfo);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                Repaint();
            }
        }
    }

}
#endif