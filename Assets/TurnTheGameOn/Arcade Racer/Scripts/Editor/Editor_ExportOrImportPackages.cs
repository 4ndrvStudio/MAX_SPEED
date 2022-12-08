namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEditor;
    using System.IO;
    using System.Collections.Generic;

    public class ExportOrImportPackages : Editor
    {
        static string InputManager_Asset = "ProjectSettings/InputManager.asset";
        static string InputManager_Playstation4Controller_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_Playstation4Controller.unitypackage";
        static string InputManager_XboxOneController_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_XboxOneController.unitypackage";
        static string InputManager_Keyboard_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_Keyboard.unitypackage";
        static string InputManager_Full_Standalone_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_Full_Standalone.unitypackage";
        static string TagManager_Asset = "ProjectSettings/TagManager.asset";
        static string TagManager_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/TagsAndLayers/TagManager.unitypackage";
        static string AvatarDriver_RCCReference_Asset = "Assets/TurnTheGameOn/Arcade Racer/Scripts/AvatarDriver_RCCReference.cs";
        static string AvatarDriver_RCCReference_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/3rdPartySupport/AvatarDriver_RCCReference.unitypackage";
        static string EditorBuildSettings_Asset = "ProjectSettings/EditorBuildSettings.asset";
        static string EditorBuildSettings_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/EditorBuildSettings/EditorBuildSettings.unitypackage";
        static string StandaloneGameTemplateProjectSettings_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/_GameTemplate/StandaloneGameTemplateProjectSettings.unitypackage";
        static string StandaloneGameTemplate_EVP_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/_GameTemplate/StandaloneGameTemplateProjectSettings_EVP.unitypackage";
        static string EVP_GameTemplate = "Assets/TurnTheGameOn/Arcade Racer/_GameTemplate_WindowsStandalone/_EVP/";
        static string MobileGameTemplateProjectSettings_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/_GameTemplate/MobileGameTemplateProjectSettings.unitypackage";
        static ExportPackageOptions flags = ExportPackageOptions.Interactive;

        #region Export StandaloneGameTemplateProjectSettings_EVP

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/StandaloneGameTemplate_EVP")]
        static void Export_StandaloneGameTemplateProjectSettings_EVP()
        {
            List<string> files = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(EVP_GameTemplate);
            FileInfo[] info = dir.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in info)
            {
                if (f.FullName.Contains(".meta") == false)
                {
                    string path = f.FullName;
                    path = path.Remove(0, 32);
                    path = path.Replace("\\", "/");
                    files.Add(path);
                }
            }
            files.Add(InputManager_Asset);
            files.Add(TagManager_Asset);
            files.Add(EditorBuildSettings_Asset);
            string[] FilesToExport = files.ToArray();
            AssetDatabase.ExportPackage(FilesToExport, StandaloneGameTemplate_EVP_ExportPath, flags);
        }

        #endregion

        #region Export StandaloneGameTemplateProjectSettings

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/StandaloneGameTemplate")]
        static void Export_StandaloneGameTemplateProjectSettings()
        {
            string[] StandaloneGameTemplateProjectSettings_ExportAssets = new string[]
            {
            InputManager_Asset,
            TagManager_Asset,
            EditorBuildSettings_Asset
            };
            AssetDatabase.ExportPackage(StandaloneGameTemplateProjectSettings_ExportAssets, StandaloneGameTemplateProjectSettings_ExportPath, flags);
        }

        #endregion

        #region Export MobileGameTemplateProjectSettings

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/MobileGameTemplate")]
        static void Export_MobileGameTemplateProjectSettings()
        {
            string[] MobileGameTemplateProjectSettings_ExportAssets = new string[]
            {
            InputManager_Asset,
            TagManager_Asset,
            EditorBuildSettings_Asset
            };
            AssetDatabase.ExportPackage(MobileGameTemplateProjectSettings_ExportAssets, MobileGameTemplateProjectSettings_ExportPath, flags);
        }

        #endregion

        #region Export InputManager

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/InputManager/XboxOneController")]
        static void Export_InputManager_XboxOneController()
        {
            AssetDatabase.ExportPackage(InputManager_Asset, InputManager_XboxOneController_ExportPath, flags);
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/InputManager/Keyboard")]
        static void Export_InputManager_Keyboard()
        {
            AssetDatabase.ExportPackage(InputManager_Asset, InputManager_Keyboard_ExportPath, flags);
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/InputManager/Playstation4Controller")]
        static void Export_InputManager_Playstation4Controller()
        {
            AssetDatabase.ExportPackage(InputManager_Asset, InputManager_Playstation4Controller_ExportPath, flags);
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/InputManager/Full_Standalone")]
        static void Export_InputManager_Full_Standalone()
        {
            AssetDatabase.ExportPackage(InputManager_Asset, InputManager_Full_Standalone_ExportPath, flags);
        }

        #endregion

        #region Export TagManager

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/TagManager")]
        static void Export_TagManager()
        {
            AssetDatabase.ExportPackage(TagManager_Asset, TagManager_ExportPath, flags);
        }

        #endregion

        #region Export EditorBuildSettings

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Export/EditorBuildSettings")]
        static void Export_EditorBuildSettings()
        {
            AssetDatabase.ExportPackage(EditorBuildSettings_Asset, EditorBuildSettings_ExportPath, flags);
        }

        #endregion

        #region Export RCC Support

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/3rd Party Support/Export/AvatarDriver_RCCReference")]
        static void Export_AvatarDriver_RCCReference()
        {
            AssetDatabase.ExportPackage(AvatarDriver_RCCReference_Asset, AvatarDriver_RCCReference_ExportPath, flags);
        }

        #endregion

        #region Import StandaloneGameTemplateProjectSettings_EVP

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/StandaloneGameTemplate_EVP")]
        static void Import_StandaloneGameTemplateProjectSettings_EVP()
        {
            AssetDatabase.ImportPackage(StandaloneGameTemplate_EVP_ExportPath, true);
        }

        #endregion

        #region Import StandaloneGameTemplateProjectSettings

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/StandaloneGameTemplate")]
        static void Import_StandaloneGameTemplateProjectSettings()
        {
            AssetDatabase.ImportPackage(StandaloneGameTemplateProjectSettings_ExportPath, true);
        }

        #endregion

        #region Import MobileGameTemplateProjectSettings

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/MobileGameTemplate")]
        static void Import_MobileGameTemplateProjectSettings()
        {
            AssetDatabase.ImportPackage(MobileGameTemplateProjectSettings_ExportPath, true);
        }

        #endregion

        #region Import InputManager

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/InputManager/Keyboard")]
        static void Import_InputManager_Keyboard()
        {
            AssetDatabase.ImportPackage(InputManager_Keyboard_ExportPath, true);
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/InputManager/Full_Standalone")]
        static void Import_InputManager_Full_Standalone()
        {
            AssetDatabase.ImportPackage(InputManager_Full_Standalone_ExportPath, true);
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/InputManager/XboxOneController")]
        static void Import_InputManager_XboxOneController()
        {
            AssetDatabase.ImportPackage(InputManager_XboxOneController_ExportPath, true);
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/InputManager/Playstation4Controller")]
        static void Import_InputManager_Playstation4Controller()
        {
            AssetDatabase.ImportPackage(InputManager_Playstation4Controller_ExportPath, true);
        }

        #endregion

        #region Import TagManager

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/TagManager")]
        static void Import_TagManager()
        {
            AssetDatabase.ImportPackage(TagManager_ExportPath, true);
        }

        #endregion

        #region Import EditorBuildSettings

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Project Settings/Import/EditorBuildSettings")]
        static void Import_EditorBuildSettings()
        {
            AssetDatabase.ImportPackage(EditorBuildSettings_ExportPath, true);
        }

        #endregion

        #region Import RCC Support

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/3rd Party Support/Import/AvatarDriver_RCCReference")]
        static void Import_AvatarDriver_RCCReference()
        {
            AssetDatabase.ImportPackage(AvatarDriver_RCCReference_ExportPath, true);
        }

        #endregion

    }
}