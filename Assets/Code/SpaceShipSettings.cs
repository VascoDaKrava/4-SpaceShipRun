using UnityEngine;

namespace SpaceShipRun.Data
{
    [CreateAssetMenu(
        fileName = "SpaceShipSettings",
        menuName = "SpaceShipRun/Settings/SpaceShipSettings"
        )]
    public sealed class SpaceShipSettings : ScriptableObject
    {
        [SerializeField, Range(0.01f, 0.1f)] private float _acceleration;
        [SerializeField, Range(1.0f, 2000.0f)] private float _shipSpeed;
        [SerializeField, Range(1.0f, 5.0f)] private int _faster;
        [SerializeField, Range(0.01f, 179.0f)] private float _normalFov = 60.0f;
        [SerializeField, Range(0.01f, 179.0f)] private float _fasterFov = 30.0f;
        [SerializeField, Range(0.1f, 5.0f)] private float _changeFovSpeed = 0.5f;

        public float Acceleration => _acceleration;
        public float ShipSpeed => _shipSpeed;
        public float Faster => _faster;
        public float NormalFov => _normalFov;
        public float FasterFov => _fasterFov;
        public float ChangeFovSpeed => _changeFovSpeed;
    }
}