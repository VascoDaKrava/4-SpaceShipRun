using SpaceShipRun.Abstraction;
using SpaceShipRun.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceShipRun.Main
{
    [Obsolete]
    public sealed class SolarSystemConstructorWindow : EditorWindow
    {
        private Transform _planetContainer;
        private PlanetOrbit _planetPrefab;

        private Editor editor;

        private int _planetsCount;
        [SerializeField, NonReorderable] private List<PlanetData> _planets = new List<PlanetData>(0);

        private Transform _asteroidsContainer;
        private CollisionObject _asteroidPrefab;

        private Vector2 _asteroidScaleRange;
        private const float ASTEROID_SCALE_MIN = 0.1f;
        private const float ASTEROID_SCALE_MAX = 2.0f;

        private float _asteroidRadius;
        private const float ASTEROID_RADIUS_MAX = 50.0f;

        private int _asteroidsCount;
        private const int ASTEROID_COUNT_MAX = 1000;

        [MenuItem("Window/SpaceShipRun/SolarSystemConstructor")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(SolarSystemConstructorWindow), false, "Solar System Constructor");
            window.minSize = new Vector2 { x = 500, y = 300 };
        }

        private void OnGUI()
        {

            #region Planets

            GUILayout.Label("Planet settings", EditorStyles.boldLabel);
            _planetsCount = EditorGUILayout.IntSlider("Planets", _planetsCount, 1, Enum.GetNames(typeof(PlanetNames)).Length);

            if (_planetsCount > _planets.Count)
            {
                _planets.Add(new PlanetData { Name = (PlanetNames)_planetsCount });
            }
            else if (_planetsCount < _planets.Count)
            {
                _planets.RemoveRange(_planetsCount, _planets.Count - _planetsCount);
            }

            if (!editor)
            {
                editor = Editor.CreateEditor(this);
            }

            if (editor)
            {
                editor.OnInspectorGUI();
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            _planetContainer = EditorGUILayout.ObjectField("Container for Planets", _planetContainer, typeof(Transform), true) as Transform;
            _planetPrefab = EditorGUILayout.ObjectField("Prefab of Planet", _planetPrefab, typeof(PlanetOrbit), true) as PlanetOrbit;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Get data from scene", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {
                GetPlanetsData();
            }

            GUILayout.Space(10.0f);

            if (GUILayout.Button("Remove Planents", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {
                ClearListAndChildren(ref _planets, ref _planetsCount, _planetContainer);
            }

            GUILayout.Space(10.0f);

            if (GUILayout.Button("Make Planents", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {
                MakePlanets();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            #endregion


            #region Asteroids

            GUILayout.Space(EditorGUIUtility.singleLineHeight * 2);

            GUILayout.Label("Asteroids settings", EditorStyles.boldLabel);
            _asteroidRadius = EditorGUILayout.Slider("Asteroid ring radius", _asteroidRadius, 1.0f, ASTEROID_RADIUS_MAX);
            _asteroidsCount = EditorGUILayout.IntSlider("Quantity of asteroids", _asteroidsCount, 1, ASTEROID_COUNT_MAX);
            EditorGUILayout.MinMaxSlider("Asteroid scale", ref _asteroidScaleRange.x, ref _asteroidScaleRange.y, ASTEROID_SCALE_MIN, ASTEROID_SCALE_MAX);
            GUILayout.Label($"{_asteroidScaleRange}");

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            _asteroidsContainer = EditorGUILayout.ObjectField("Container for Asteroids", _asteroidsContainer, typeof(Transform), true) as Transform;
            _asteroidPrefab = EditorGUILayout.ObjectField("Asteroid prefab", _asteroidPrefab, typeof(CollisionObject), false) as CollisionObject;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Clear Asteroids", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {
                ClearListAndChildren(_asteroidsContainer);
            }

            GUILayout.Space(100.0f);

            if (GUILayout.Button("Make Asteroids", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {
                MakeAsteroids();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            #endregion

        }

        private void GetPlanetsData()
        {
            List<IPlanetData> planetsList = new List<IPlanetData>(FindObjectsOfType<PlanetOrbit>());

            _planetsCount = planetsList.Count;
            var planets = new List<PlanetData>(_planetsCount);

            foreach (var item in planetsList)
            {
                planets.Add(
                    new PlanetData
                    {
                        Name = item.Name,
                        OrbitRadius = item.OrbitRadius,
                        SecondsForFullCircle = item.SecondsForFullCircle
                    });
            }

            _planets = new List<PlanetData>(planets.OrderBy(item => item.Name));
        }

        private void ClearListAndChildren(Transform container)
        {
            var childrenObjects = container.childCount;

            for (int i = childrenObjects - 1; i >= 0; i--)
            {
                DestroyImmediate(container.transform.GetChild(i).gameObject);
            }
        }

        private void ClearListAndChildren<T>(ref List<T> list, ref int count, Transform container)
        {
            list.Clear();
            count = 1;

            ClearListAndChildren(container);
        }

        private void MakePlanets()
        {
            foreach (var item in _planets)
            {
                var currentPlanet = Instantiate(_planetPrefab, _planetContainer);
                currentPlanet.Name = item.Name;
                currentPlanet.OrbitRadius = item.OrbitRadius;
                currentPlanet.SecondsForFullCircle = item.SecondsForFullCircle;
                currentPlanet.name = item.Name.ToString();

                EditorUtility.SetDirty(currentPlanet);
            }
        }

        private void MakeAsteroids()
        {
            for (int i = 0; i < _asteroidsCount; i++)
            {
                var alfa = Random.Range(0, 2 * Mathf.PI);
                var x = _asteroidRadius * Mathf.Sin(alfa);
                var y = _asteroidRadius * Mathf.Cos(alfa);

                var scale = Random.Range(_asteroidScaleRange.x, _asteroidScaleRange.y);

                Instantiate(_asteroidPrefab, new Vector3 { x = x, y = 0.0f, z = y }, Random.rotation, _asteroidsContainer).transform.localScale = Vector3.one * scale;
            }

            EditorUtility.SetDirty(_asteroidPrefab);
        }
    }
}