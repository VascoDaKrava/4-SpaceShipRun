using UnityEditor;
using UnityEngine;

namespace SpaceShipRun.Main
{
    [CustomPropertyDrawer(typeof(PlanetData))]
    public class PlanetDataDrawer : PropertyDrawer
    {
        private const int SPACE = 50;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // position - просто Rect x: 3.00; y: 119.0 и что-то похожее на "размер" окна width и height
            // property - UnityEditor.SerializedProperty
            // label - UnityEngine.GUIContent
            var planetNameProperty = property.FindPropertyRelative("Name");
            var orbitRadiusProperty = property.FindPropertyRelative("OrbitRadius");
            var rotationPerSecondProperty = property.FindPropertyRelative("FullCirclePerSecond");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel = 0;

            position.width = (position.width - SPACE) / 5.0f;
            EditorGUI.LabelField(position, planetNameProperty.enumNames[planetNameProperty.enumValueIndex]);

            position.x += position.width;
            EditorGUI.LabelField(position, orbitRadiusProperty.displayName);

            position.x += SPACE;
            EditorGUI.PropertyField(position, orbitRadiusProperty, GUIContent.none);

            position.x += position.width + SPACE;
            EditorGUI.LabelField(position, rotationPerSecondProperty.displayName);
            position.x += SPACE * 1.5f;
            EditorGUI.PropertyField(position, rotationPerSecondProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}