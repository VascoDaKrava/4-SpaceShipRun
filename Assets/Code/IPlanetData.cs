using SpaceShipRun.Main;

namespace SpaceShipRun.Abstraction
{
    public interface IPlanetData
    {
        PlanetNames Name { get; set; }
        float OrbitRadius { get; set; }
        float SecondsForFullCircle { get; set; }
    }
}