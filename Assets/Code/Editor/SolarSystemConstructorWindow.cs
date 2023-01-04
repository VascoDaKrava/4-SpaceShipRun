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
        [SerializeField, NonReorderable] private List<PlanetData> _planets;

        [MenuItem("Window/SpaceShipRun/SolarSystemConstructor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SolarSystemConstructorWindow), false, "Solar System Constructor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Settings", EditorStyles.boldLabel);
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

            GUILayout.Space(EditorGUIUtility.singleLineHeight * 2);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Planents"))
            {

            }

            if (GUILayout.Button("Make Planents"))
            {

            }
            GUILayout.EndHorizontal();
        }
    }
}