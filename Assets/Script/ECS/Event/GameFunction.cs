using System;
using UnityEngine;
namespace Donuts
{
    public partial class GameFunction
    {
        public Func<EntityComponent, Entity> onSpawnEntityFromPool;
        public Func<GameObject, Transform, GameObject> onSpawnGameObjectFromPool;

        public GameFunction()
        {
            onSpawnEntityFromPool = DefaultSpawnEntity;
            onSpawnGameObjectFromPool = DefaultSpawnGameObject;
        }

        public static GameObject DefaultSpawnGameObject(GameObject prefab, Transform parent)
        {
            return GameObject.Instantiate(prefab, parent);
        }

        public static Entity DefaultSpawnEntity(EntityComponent prefab)
        {
            EntityComponent entityComponent = GameObject.Instantiate<EntityComponent>(prefab);
            return entityComponent.ToEntity();
        }

    }
}