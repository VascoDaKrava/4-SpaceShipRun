using System;


namespace SpaceShipRun.Main
{
    [Serializable]
    public struct PlanetData
    {
        public PlanetNames Name;
        public float OrbitRadius;
        public float FullCirclePerSecond;
    }
}