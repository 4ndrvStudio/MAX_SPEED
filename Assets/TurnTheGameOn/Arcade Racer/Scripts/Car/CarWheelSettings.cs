﻿namespace TurnTheGameOn.ArcadeRacer
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CarWheelSettings", menuName = "TurnTheGameOn/Arcade Racer/CarWheelSettings")]
    public class CarWheelSettings : ScriptableObject
    {
        public bool enableSkid;
        public bool enableSkidAudio;
        [Range(0, 1)] public float slipLimit;
        [System.Serializable]
        public class SkidTrailProfile
        {
            public string name;
            public Gradient smokeColor;
            public Transform SkidTrailPrefab;
        }
        public List<SkidTrailProfile> skidTrailProfiles;
    }
}