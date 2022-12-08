
namespace TurnTheGameOn.ArcadeRacer
{
    [System.Serializable]
    public struct CarAISensorInfo
    {
        public string name;
        public string layer;
        public string tag;
        public bool hit;
        public float distance;
        public CarAISensor sensor;
    }
}