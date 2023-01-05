using SpaceShipRun.Abstraction;
using SpaceShipRun.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpaceShipRun.Main
{
    public sealed class SolarSystemConstructorWindow : EditorWindow
    {
        private Transform _planetContainer;
        private PlanetOrbit _planetPrefab;

        private Transform _asteroidsContainer;
        private GameObject _asteroidPrefab;

        private Editor editor;

        private int _planetsCount;
        [SerializeField, NonReorderable] private List<PlanetData> _planets = new List<PlanetData>(0);

        private float _asteroidRadius;
        private const float ASTEROID_RADIUS_MAX = 50.0f;

        private int _asteroidsDensity;
        private const int ASTEROID_DENSITY_MAX = 10;

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
            _asteroidsDensity = EditorGUILayout.IntSlider("Density of asteroids", _asteroidsDensity, 1, ASTEROID_DENSITY_MAX);

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Clear Asteroids", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {

            }

            GUILayout.Space(100.0f);

            if (GUILayout.Button("Make Asteroids", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {

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

        private void ClearListAndChildren<T>(ref List<T> list, ref int count, Transform container)
        {
            list.Clear();
            count = 1;

            var childrenObjects = container.childCount;

            for (int i = childrenObjects - 1; i >= 0; i--)
            {
                DestroyImmediate(container.transform.GetChild(i).gameObject);
            }
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
    }
}