using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
namespace Donuts
{
    [CustomPropertyDrawer(typeof(ElementNameByClassAttribute))]
    public class ElementNameByClassAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            System.Type t = property.GetValue<object>().GetType();

            EditorGUI.PropertyField(rect, property, new GUIContent(t.Name), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}