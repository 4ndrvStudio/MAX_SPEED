namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ProjectInfo", menuName = "TurnTheGameOn/Arcade Racer/ProjectInfo")]
	public class ProjectInfo : ScriptableObject 
	{
        public bool useLaunchWindow;
        public string assetVersion;
        [HideInInspector] public bool completedWelcome { get; private set; }
        public void CompletedWelcome()
        {
            completedWelcome = true;
        }
        public void ResetWelcome()
        {
            completedWelcome = false;
        }
    }
}