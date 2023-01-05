using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace SpaceShipRun.Main
{
    public class AsteroidsController : MonoBehaviour
    {
        private List<Asteroid> _asteroids;
        private List<Vector3> _positions;

        [SerializeField, Range(1.0f, 25.0f)] private float _rotationTime;
        [SerializeField] private Transform _sun;
        private float _radius;
        private NativeArray<Vector3> _positionsNative;
        private NativeArray<Vector3> _positionsFinal;
        private JobHandle _jobHandle;

        private struct MoveJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Vector3> PositionsNative;
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public float Radius;
            [ReadOnly] public float RotationTime;

            [WriteOnly] public NativeArray<Vector3> FinalPositions;

            public void Execute(int index)
            {
                float currentAngleRad;
                var currentAngleRadsin = Mathf.Asin(PositionsNative[index].z / Radius);

                if (currentAngleRadsin > 0)
                {
                    currentAngleRad = Mathf.Acos(PositionsNative[index].x / Radius);
                }
                else
                {
                    currentAngleRad = 2 * Mathf.PI - Mathf.Acos(PositionsNative[index].x / Radius);
                }

                var deltaRad = 2 * Mathf.PI * DeltaTime / RotationTime;

                var alfa = currentAngleRad + deltaRad;

                var x = Mathf.Cos(alfa) * Radius;
                var z = Mathf.Sin(alfa) * Radius;
                
                FinalPositions[index] = new Vector3(x, 0.0f, z);
            }
        }

        private void Start()
        {
            _asteroids = new List<Asteroid>(FindObjectsOfType<Asteroid>());
            _positions = new List<Vector3>(_asteroids.Count);
            foreach (var item in _asteroids)
            {
                _positions.Add(item.transform.position);
            }
            _radius = Vector3.Distance(_sun.position, _asteroids[0].transform.position);
            StartJob();
        }

        private void Update()
        {
            if (_jobHandle.IsCompleted)
            {
                _jobHandle.Complete();
                _positionsNative.Dispose();

                for (int i = 0; i < _asteroids.Count; i++)
                {
                    _positions[i] = _positionsFinal[i];
                    _asteroids[i].transform.position = _positionsFinal[i];
                }
                
                _positionsFinal.Dispose();
                StartJob();
            }
        }

        private void StartJob()
        {
            _positionsNative = new NativeArray<Vector3>(_positions.ToArray(), Allocator.Persistent);
            _positionsFinal = new NativeArray<Vector3>(_asteroids.Count, Allocator.Persistent);

            var moveTask = new MoveJob
            {
                DeltaTime = Time.deltaTime,
                PositionsNative = _positionsNative,
                Radius = _radius,
                RotationTime = _rotationTime,
                FinalPositions = _positionsFinal
            };

            _jobHandle = moveTask.Schedule(_positionsNative.Length, 32);
        }
    }
}