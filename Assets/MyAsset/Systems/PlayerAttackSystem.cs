using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class PlayerAttackGroup : ComponentGroup<PlayerAttackGroup, PlayerAttackComponent, CharacterMoveComponent> { }

public class PlayerAttackSystem : AGameSystem, IUpdateSystem
{
    public override void SetupEvents()
    {
        masterSystem.gameEvent.onOneTouchMove += onTouchStay;
    }

    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<PlayerAttackGroup>(AttackTimer, deltaTime);
    }

    private void AttackTimer(PlayerAttackGroup group, float deltaTime)
    {
        group.data1.timer += deltaTime;
        if (group.data1.timer < group.data1.interval) return;
        if (group.data1.isCanShot) return;
        group.data1.isCanShot = true;
        group.data1.timer = 0.0f;
    }

    private void onTouchStay(Vector3 pos)
    {
        entityManager.Foreach<PlayerAttackGroup>(PlayerAttack);
    }

    private void PlayerAttack(PlayerAttackGroup group)
    {
        if (!group.data1.isCanShot) return;
        Entity bullet = gameFunction.onSpawnEntityFromPool?.Invoke(Singleton.instance.resourceData.bulletPrefab.GetComponent<EntityComponent>());
        gameEvent.onSpawnedEntity?.Invoke(bullet);
        bullet.transform.position = group.entity.transform.position;
        bullet.GetComponent<BulletMoveComponent>().direction = group.entity.transform.forward;
        group.data1.isCanShot = false;
    }
}
