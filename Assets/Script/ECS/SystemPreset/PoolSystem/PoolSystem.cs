using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{

    public class PoolSystem : AGameSystem
    {
        private Dictionary<int, List<Entity>> pool = new Dictionary<int, List<Entity>>();


        public override void SetupEvents()
        {
            gameFunction.onSpawnEntityFromPool = OnSpawnEntityFromPool;
            gameEvent.onRemovedEntity += OnRemovedEntity;
        }

        private Entity OnSpawnEntityFromPool(EntityComponent prefab)
        {
            int hash = prefab.GetHashCode();
            if (pool.ContainsKey(hash))
            {
                List<Entity> targetList = pool[hash];
                int count = targetList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (targetList[i].gameObject.activeSelf == false)
                    {
                        targetList[i].gameObject.SetActive(true);
                        return targetList[i];
                    }
                }
                EntityComponent original = targetList[0].gameObject.GetComponent<EntityComponent>();
                EntityComponent eC = GameObject.Instantiate<EntityComponent>(original);
                Entity instance = eC.ToEntity();
                targetList.Add(instance);
                return instance;
            }

            EntityComponent entityComponent = GameObject.Instantiate<EntityComponent>(prefab);
            List<Entity> poolList = new List<Entity>();

            Entity entity = entityComponent.ToEntity();
            poolList.Add(entity);
            pool.Add(hash, poolList);
            return entity;
        }

        private void OnRemovedEntity(Entity entity)
        {
            entity.gameObject.SetActive(false);

        }
    }
}