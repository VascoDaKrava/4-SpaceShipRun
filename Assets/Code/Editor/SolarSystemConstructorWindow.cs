using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpaceShipRun.Main
{
    public sealed class SolarSystemConstructorWindow : EditorWindow
    {
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

            if (_planets.Count != _planetsCount)
            {
                _planets = new List<PlanetData>(_planetsCount);

                for (int i = 0; i < _planetsCount; i++)
                {
                    _planets.Add(new PlanetData { Name = (PlanetNames)i + 1 });
                }
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

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Clear Planents", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {

            }

            GUILayout.Space(100.0f);

            if (GUILayout.Button("Make Planents", GUILayout.Width(150.0f), GUILayout.Height(30.0f)))
            {

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
    }
}