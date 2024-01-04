using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Donuts
{
    //do not use monobehavior directly because of order of components need to adjudge in runtime
    //monobehavior serialization will keep the order of serializing
    public class Entity 
    {
        public GameObject gameObject;
        public Transform transform;

        public IComponent[] components = new IComponent[ECSDefinition.TypeNumbers];
        public int[] groupIds = new int[ECSDefinition.groupNumbers];
        private long bits = 0;

        public T GetComponent<T>() where T : IComponent
        {
            return (T)components[ECSDefinition.GetComponentId<T>()];
        }

        public void AddComponent(IComponent component)
        {
            int id = ECSDefinition.GetComponentId(component.GetType());
            components[id] = component;
            long bit = 1 << id;
            bits = bits | bit;
        }


        public void RemoveComponent<T>() where T : IComponent
        {
            long bit = ECSDefinition.GetComponentBit<T>();
            bits = bits - (bits & bit);
            components[ECSDefinition.GetComponentId<T>()] = null;
        }

        public void RemoveComponent(IComponent component)
        {
            long bit = ECSDefinition.GetComponentBit(component.GetType());
            bits = bits - (bits & bit);
            components[ECSDefinition.GetComponentId(component.GetType())] = null;
        }

        public bool HasComponents(long targetbits)
        {
            return (bits & targetbits) == targetbits;
        }

        public bool HasComponents(params System.Type[] types)
        {
            long bit = 0;
            int length = types.Length;
            for (int i = 0; i < length; i++)
            {
                bit = bit | ECSDefinition.GetComponentBit(types[i]);
            }
            return (bits & bit) == bit;
        }

    }
}