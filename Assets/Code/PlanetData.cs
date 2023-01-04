using System;
using UnityEngine;

namespace SpaceShipRun.Main
{
    [Serializable]
    public sealed class PlanetData
    {
        public PlanetNames Name;
        [Range(1.0f, 50.0f)] public float OrbitRadius = 1.0f;
        [Range(0.001f, 1.0f)] public float SecondsForFullCircle = 0.005f;
    }
}