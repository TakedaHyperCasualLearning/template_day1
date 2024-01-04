using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Donuts
{

    public class ComponentChunk
    {
        private int groupSlotID = 0;
        private long bits = 0;

        private ComponentGroup model;
        private List<ComponentGroup> groups = new List<ComponentGroup>();
        private List<int> bannedIndex = new List<int>();
        //private List<int> validateIndex = new List<int>(); can not be used because of when removed has to remove this list

        public ComponentChunk(ComponentGroup groupModel, int slotID)
        {
            groupSlotID = slotID;
            model = groupModel;
            bits = model.GetTargetBits();
        }

        public void OnDestroy()
        {
            groups.Clear();
        }

        public void AddEntity(Entity entity)
        {
            bool isTarget = entity.HasComponents(bits);
            if (isTarget == false)
            {
                return;
            }
            if (bannedIndex.Count > 0)
            {
                int index = bannedIndex[bannedIndex.Count - 1];
                groups[index].entity = entity;
                entity.groupIds[groupSlotID] = index;
                groups[index].isActivated = true;
                groups[index].Init(entity);
                bannedIndex.RemoveAt(bannedIndex.Count - 1);
                return;
            }
            ComponentGroup groupInstance = model.CloneModel(entity);
            groupInstance.entity = entity;
            int id = groups.Count;
            entity.groupIds[groupSlotID] = id;
            groups.Add(groupInstance);
        }

        public void RemoveEntity(Entity entity)
        {
            bool isTarget = entity.HasComponents(bits);
            if (isTarget == false)
            {
                return;
            }
            int index = entity.groupIds[groupSlotID];
            groups[index].isActivated = false;
            bannedIndex.Add(index);
        }

        public bool IsTarget(Entity entity)
        {
            return entity.HasComponents(bits);
        }

        public int GetEntityNumbers()
        {
            return groups.Count - bannedIndex.Count;
        }

        public void Iterate<T>(Action<T> callBack) where T : ComponentGroup
        {
            int count = groups.Count;

            for (int i = 0; i < count; i++)
            {
                if (groups[i].isActivated)
                {
                    callBack((T)groups[i]);
                }

            }
        }

        public void Iterate<T>(Action<T, float> callBack, float deltaTime) where T : ComponentGroup
        {
            int count = groups.Count;

            for (int i = 0; i < count; i++)
            {
                if (groups[i].isActivated)
                {
                    callBack((T)groups[i], deltaTime);
                }
            }
        }

        public void Iterate<T1, T2>(Action<T1, T2> callBack, T2 amount) where T1 : ComponentGroup
        {
            int count = groups.Count;

            for (int i = 0; i < count; i++)
            {
                if (groups[i].isActivated)
                {
                    callBack((T1)groups[i], amount);
                }
            }
        }

        public List<Entity> GetEntityCloseToPoint(Vector3 position, float distance)
        {
            int count = groups.Count;
            float squarDistance = distance * distance;
            List<Entity> target = new List<Entity>();
            for (int i = 0; i < count; i++)
            {
                if (groups[i].isActivated && (groups[i].entity.transform.position - position).sqrMagnitude <= squarDistance)
                {
                    target.Add(groups[i].entity);

                }
            }
            return target;
        }

        public Entity GetClosestEntityToPoint(Vector3 position)
        {
            int count = groups.Count;
            float minDistance = Mathf.Infinity;
            float distance;
            Entity target = null;
            for (int i = 0; i < count; i++)
            {

                if (groups[i].isActivated)
                {
                    distance = (groups[i].entity.transform.position - position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = groups[i].entity;
                    }
                }
            }
            return target;
        }

        public Entity GetFirstEntity()
        {
            int count = groups.Count;
            for (int i = 0; i < count; i++)
            {

                if (groups[i].isActivated)
                {
                    return groups[i].entity;
                }
            }
            return null;
        }
    }

    //entity manager
}