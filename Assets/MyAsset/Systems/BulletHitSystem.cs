using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class BulletHitGroup : ComponentGroup<BulletHitGroup, BulletAttackComponent> { }

public class BulletHitSystem : AGameSystem, IUpdateSystem
{
    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<BulletHitGroup>(HitBullet);
    }

    private void HitBullet(BulletHitGroup group)
    {
        entityManager.Foreach<EnemyMoveGroup, BulletHitGroup>(CheckCollision, group);
    }

    private void CheckCollision(EnemyMoveGroup enemyGroup, BulletHitGroup bulletGroup)
    {
        float radius = enemyGroup.entity.transform.localScale.x + bulletGroup.entity.transform.localScale.x;
        float distance = Vector3.Distance(enemyGroup.entity.transform.position, bulletGroup.entity.transform.position);

        if (radius < distance) return;

        gameEvent.onRemovedEntity?.Invoke(bulletGroup.entity);
        enemyGroup.entity.GetComponent<DamageComponent>().damagePoint = bulletGroup.data1.attackPoint;
    }
}
