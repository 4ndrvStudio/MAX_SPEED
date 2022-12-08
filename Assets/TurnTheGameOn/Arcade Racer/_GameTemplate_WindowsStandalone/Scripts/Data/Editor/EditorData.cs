namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEditor;

    public static class EditorData
    {
        private static ProjectInfo m_ProjectInfo;
        public static ProjectInfo ProjectInfo
        {
            get
            {
                if (m_ProjectInfo == null)
                {
                    var guids = AssetDatabase.FindAssets("t:TurnTheGameOn.ArcadeRacer.ProjectInfo");
                    if (guids.Length > 0)
                    {
                        var asset = AssetDatabase.LoadAssetAtPath<ProjectInfo>(AssetDatabase.GUIDToAssetPath(guids[0]));
                        m_ProjectInfo = asset;
                    }

                }
                return m_ProjectInfo;
            }
        }
    }
}