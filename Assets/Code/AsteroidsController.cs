using System.Collections.Generic;
using UnityEngine;

namespace SpaceShipRun.Main
{
    public class AsteroidsController : MonoBehaviour
    {
        private List<Asteroid> _asteroids;

        [SerializeField, Range(1.0f, 25.0f)] private float _rotationTime;
        [SerializeField] private Transform _sun;
        private float _radius;
        private float _alfa;
        private bool _left;
        private Vector3 vector = Vector3.right;

        private void Start()
        {
            _asteroids = new List<Asteroid>(FindObjectsOfType<Asteroid>());
            _radius = Vector3.Distance(_sun.position, _asteroids[0].transform.position);
        }

        private void Update()
        {
            foreach (var item in _asteroids)
            {
                //item.transform.rotation = Random.rotation;
                float currentAngleRad;
                var currentAngleRadsin = Mathf.Asin(item.transform.position.z / _radius);

                if (currentAngleRadsin > 0)
                {
                    currentAngleRad = Mathf.Acos(item.transform.position.x / _radius);
                }
                else
                {
                    currentAngleRad = 2 * Mathf.PI - Mathf.Acos(item.transform.position.x / _radius);
                }

                var deltaRad = 2 * Mathf.PI * Time.deltaTime / _rotationTime;

                _alfa = currentAngleRad + deltaRad;

                var x = Mathf.Cos(_alfa) * _radius;
                var z = Mathf.Sin(_alfa) * _radius;
                item.transform.position = new Vector3(x, 0.0f, z);
            }
        }
    }
}