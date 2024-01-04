using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class EnemyMoveGroup : ComponentGroup<EnemyMoveGroup, EnemyMoveComponent, CharacterMoveComponent> { }
public class EnemyMoveSystem : AGameSystem, IUpdateSystem
{
    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<EnemyMoveGroup>(MoveEnemy);
    }

    private void MoveEnemy(EnemyMoveGroup group)
    {
        group.data2.targetPosition = gameStat.playerGroup.entity.transform.position;
        Vector3 direction = (group.data2.targetPosition - group.entity.transform.position).normalized;
        group.data2.direction = (group.data2.targetPosition - group.entity.transform.position).normalized;
    }
}
