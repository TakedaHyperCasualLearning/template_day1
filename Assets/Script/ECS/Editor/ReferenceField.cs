using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Reflection;
using System.Linq;
using UnityEditor;
namespace Donuts
{
    public class ReferenceField<T>
    {
        private Type[] targetTypes;
        private string[] typeNames;
        public void Init(List<Type> excludedTypes)
        {
            List<Type> tempTypes = new List<Type>();
            var parentType = typeof(T);

            tempTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => parentType.IsAssignableFrom(p)).ToArray());

            List<Type> typeLists = new List<Type>();
            List<string> names = new List<string>();
            typeLists.Add(null);
            names.Add("==== Select to add one =====");
            int lenght = tempTypes.Count;
            for (int i = 0; i < lenght; i++)
            {
                if (excludedTypes.Contains(tempTypes[i]) || tempTypes[i].IsGenericType)
                {
                    continue;
                }
                names.Add(tempTypes[i].Name);
                typeLists.Add(tempTypes[i]);

            }

            targetTypes = typeLists.ToArray();
            typeNames = names.ToArray();
        }

        public bool DrawField(string label, out T targetInstance, GameObject obj = null)
        {
            int index = EditorGUILayout.Popup(label, 0, typeNames);
            if (index != 0)
            {
                targetInstance = (T)Activator.CreateInstance(targetTypes[index]);
                return true;
            }

            targetInstance = default(T);
            return false;
        }
    }
}