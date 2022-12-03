using SpaceShipRun.Data;
using UnityEngine;

namespace SpaceShipRun.Main
{
    public class SettingsContainer : Singleton<SettingsContainer>
    {
        [SerializeField] private SpaceShipSettings _spaceShipSettings;

        public SpaceShipSettings SpaceShipSettings => _spaceShipSettings;
    }
}