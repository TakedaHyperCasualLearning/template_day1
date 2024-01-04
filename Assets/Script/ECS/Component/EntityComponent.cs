using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Donuts
{
    public class EntityComponent : MonoBehaviour
    {
        [ElementNameByClassAttribute]
        [SerializeReference]
        public List<IComponent> components = new List<IComponent>();
        public Entity owner;
        public Entity ToEntity()
        {
            if(owner != null)
            {
                return owner;
            }
            Entity entity = new Entity();
            entity.gameObject = gameObject;
            entity.transform = transform;
            owner = entity;

            int count = components.Count;
            for (int i = 0; i < count; i++)
            {
                entity.AddComponent(components[i]);
                components[i].owner = entity;
            }
            
            return entity;
        }
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            int count = components.Count;
            for (int i = 0; i < count; i++)
            {
                if(components[i] == null)
                {
                    continue;
                }
                IDrawGizmos drawGizmos = components[i] as IDrawGizmos;
                if(drawGizmos == null)
                {
                    continue;
                }
                if(drawGizmos.shouldDrawGizmos)
                {
                    drawGizmos.OnDrawGizmos(gameObject);
                }
                
            }
        }
#endif
    }
}