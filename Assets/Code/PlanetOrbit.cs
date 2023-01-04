using SpaceShipRun.Abstraction;
using SpaceShipRun.Enums;
using SpaceShipRun.Main;
using SpaceShipRun.Network;
using System;
using UnityEngine;

namespace SpaceShipRun.Mechanics
{
    [Obsolete]
    public sealed class PlanetOrbit : NetworkMovableObject, IPlanetData
    {
        protected override float _speed => _smoothTime;


        [SerializeField] private Transform _aroundPoint;
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private float _circleInSecond = 1.0f;
        [SerializeField] private float _offsetSin = 1.0f;
        [SerializeField] private float _offsetCos = 1.0f;
        [SerializeField] private float _rotationSpeed;

        private float _dist;
        private float _currentAng;
        private Vector3 _currentPositionSmoothVelocity;
        private float _currentRotationAngle;
        private const float _circleRadians = Mathf.PI * 2.0f;

        [field: SerializeField] public PlanetNames Name { get; set; }
        public float OrbitRadius
        {
            get => gameObject.transform.position.x;
            set => gameObject.transform.position =
                new Vector3
                {
                    x = value,
                    y = gameObject.transform.position.y,
                    z = gameObject.transform.position.z
                };
        }
        public float SecondsForFullCircle { get => _circleInSecond; set => _circleInSecond = value; }

        private void Start()
        {
            if (isServer)
            {
                _dist = (transform.position - _aroundPoint.position).magnitude;
            }

            Initiate(UpdatePhase.FixedUpdate);
        }

        protected override void HasAuthorityMovement()
        {
            if (!isServer)
            {
                return;
            }

            var p = _aroundPoint.position;
            p.x += Mathf.Sin(_currentAng) * _dist * _offsetSin;
            p.z += Mathf.Cos(_currentAng) * _dist * _offsetCos;

            transform.position = p;

            _currentRotationAngle += Time.deltaTime * _rotationSpeed;
            _currentRotationAngle = Mathf.Clamp(_currentRotationAngle, 0, 361);

            if (_currentRotationAngle >= 360)
            {
                _currentRotationAngle = 0;
            }

            transform.rotation = Quaternion.AngleAxis(_currentRotationAngle, transform.up);
            _currentAng += _circleRadians * _circleInSecond * Time.deltaTime;

            SendToServer();
        }

        protected override void SendToServer()
        {
            _serverPosition = transform.position;
            _serverEuler = transform.eulerAngles;
        }

        protected override void FromServerUpdate()
        {
            if (!isClient)
            {
                return;
            }

            transform.position = Vector3.SmoothDamp(transform.position,
            _serverPosition, ref _currentPositionSmoothVelocity, _speed);
            transform.rotation = Quaternion.Euler(_serverEuler);
        }
    }
}