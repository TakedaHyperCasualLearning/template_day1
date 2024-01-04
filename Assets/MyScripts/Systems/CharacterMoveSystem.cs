using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Donuts;

public class CharacterMoveGroup : ComponentGroup<CharacterMoveGroup, CharacterMoveComponent> { }

public class CharacterMoveSystem : AGameSystem, IUpdateSystem
{
    public override void SetupEvents()
    {
        base.SetupEvents();
    }

    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<CharacterMoveGroup>(MoveCharacter, deltaTime);
    }

    public void MoveCharacter(CharacterMoveGroup group, float deltaTime)
    {
        group.entity.transform.LookAt(group.data1.targetPosition);

        Vector3 direction = new Vector3(group.data1.direction.x, 0.0f, group.data1.direction.y);
        group.entity.transform.position += direction * group.data1.speed * deltaTime;
    }
}
