using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Reflection;
using System.Linq;

namespace Donuts
{
    [CustomEditor(typeof(EntityComponent))]
    public class EntityComponentEditor : Editor
    {
        private ReferenceField<IComponent> referenceField;
        private EntityComponent entityComponent;

        private void OnEnable()
        {
            entityComponent = target as EntityComponent;
            List<Type> excludedComponents = new List<Type>();
            excludedComponents.Add(typeof(IComponent));
            referenceField = new ReferenceField<IComponent>();
            referenceField.Init(excludedComponents);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            IComponent instance;
            if(referenceField.DrawField("Add Component", out instance, entityComponent.gameObject))
            {
                AddComponent(instance);
            }
        }

        private void AddComponent(IComponent component)
        {
            Type t = component.GetType();
            int count = entityComponent.components.Count;
            for (int i = 0; i < count; i++)
            {
                Type tar = entityComponent.components[i].GetType();
                if(tar == t)
                {
                    return;
                }
            }
            Undo.RecordObject(target, "Added Component");
            entityComponent.components.Add(component);
        }
    }
}