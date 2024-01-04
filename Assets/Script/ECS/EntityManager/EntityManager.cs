using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
namespace Donuts
{
    public class EntityManager
    {
        private ComponentChunk[] componentChunks;
        private int groupNumbers = 0;
        public EntityManager(GameEvent gameEvent)
        {
            groupNumbers = ECSDefinition.groupNumbers;
            componentChunks = new ComponentChunk[groupNumbers];
            var parentType = typeof(ComponentGroup);

            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => parentType.IsAssignableFrom(p)).ToArray();
            int length = types.Length;

            for (int i = 0; i < length; i++)
            {
                if (types[i] == parentType || types[i].IsGenericType)
                {
                    continue;
                }

                ComponentGroup model = Activator.CreateInstance(types[i]) as ComponentGroup;
                int index = ECSDefinition.GetComponentGroupIDSlot(types[i]);
                ComponentChunk chunk = new ComponentChunk(model, index);
                componentChunks[index] = chunk;
            }

            gameEvent.onSpawnedEntity += AddEntity;
            gameEvent.onRemovedEntity += RemoveEntity;
        }

        public void OnDestroy()
        {
            int chunck = componentChunks.Length;
            for (int i = 0; i < chunck; i++)
            {
                componentChunks[i].OnDestroy();
            }
        }

        public void AddEntity(Entity entity)
        {
            for (int i = 0; i < groupNumbers; i++)
            {
                componentChunks[i].AddEntity(entity);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            for (int i = 0; i < groupNumbers; i++)
            {
                componentChunks[i].RemoveEntity(entity);
            }
        }

        public int GetEntityNumbers<T>() where T : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T));
            return componentChunks[id].GetEntityNumbers();
        }

        public void Foreach<T>(Action<T> callback) where T : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T));
            componentChunks[id].Iterate(callback);
        }

        public void Foreach<T>(Action<T, float> callback, float deltaTime) where T : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T));
            componentChunks[id].Iterate(callback, deltaTime);
        }

        public void Foreach<T1, T2>(Action<T1, T2> callback, T2 amount) where T1 : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T1));
            componentChunks[id].Iterate(callback, amount);
        }

        public Entity GetFirst<T>() where T : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T));
            return componentChunks[id].GetFirstEntity();
        }

        public List<Entity> GetEntitiesCloseToPoint<T>(Vector3 position, float distance) where T : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T));
            return componentChunks[id].GetEntityCloseToPoint(position, distance);
        }

        public Entity GetClosestEntityToPoint<T>(Vector3 position) where T : ComponentGroup
        {
            int id = ECSDefinition.GetComponentGroupIDSlot(typeof(T));
            return componentChunks[id].GetClosestEntityToPoint(position);
        }
    }
}