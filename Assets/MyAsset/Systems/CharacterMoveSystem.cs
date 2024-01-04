using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Donuts;

public class CharacterMoveGroup : ComponentGroup<CharacterMoveGroup, CharacterMoveComponent> { }

public class CharacterMoveSystem : AGameSystem, IUpdateSystem
{
    public override void SetupEvents()
    {
    }

    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<CharacterMoveGroup>(MoveCharacter, deltaTime);
    }

    public void MoveCharacter(CharacterMoveGroup group, float deltaTime)
    {
        group.entity.transform.LookAt(group.data1.targetPosition);

        group.entity.transform.position += group.data1.direction * group.data1.speed * deltaTime;
        // group.entity.transform.Translate(group.data1.direction * group.data1.speed * deltaTime, Space.Self);
    }
}
