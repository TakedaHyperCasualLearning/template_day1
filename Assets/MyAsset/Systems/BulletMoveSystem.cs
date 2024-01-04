using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class BulletMoveGroup : ComponentGroup<BulletMoveGroup, BulletMoveComponent> { }

public class BulletMoveSystem : AGameSystem, IUpdateSystem
{
    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<BulletMoveGroup>(MoveBullet, deltaTime);
    }

    private void MoveBullet(BulletMoveGroup group, float deltaTime)
    {
        group.entity.transform.position += group.data1.direction * group.data1.speed * deltaTime;
    }
}
