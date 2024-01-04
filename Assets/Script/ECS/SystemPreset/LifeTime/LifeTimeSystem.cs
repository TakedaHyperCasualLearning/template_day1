using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{
    public class LifeTimeComponent : IComponent
    {
        public Entity owner { get; set; }
        public float lifetime = 3;
        public float timeCount = 0;
    }

    public class LifeTimeGroup : ComponentGroup
    {
        public LifeTimeComponent lifeTimeComponent;
        public float timeCount = 0;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new LifeTimeGroup();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<LifeTimeComponent>();
        }

        public override void Init(Entity entity)
        {
            lifeTimeComponent = entity.GetComponent<LifeTimeComponent>();
            lifeTimeComponent.timeCount = 0;
        }
    }

    public class LifeTimeSystem : AGameSystem, IUpdateSystem
    {

        public void OnUpdate(float deltaTime)
        {
            masterSystem.entityManager.Foreach<LifeTimeGroup>(Iterate, deltaTime);
        }

        private void Iterate(LifeTimeGroup group, float deltaTime)
        {
            group.lifeTimeComponent.timeCount += deltaTime;
            if (group.lifeTimeComponent.timeCount >= group.lifeTimeComponent.lifetime)
            {
                gameEvent.onRemovedEntity.Invoke(group.lifeTimeComponent.owner);
            }

        }

    }
}