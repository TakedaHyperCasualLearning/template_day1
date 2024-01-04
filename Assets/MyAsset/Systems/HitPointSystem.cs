using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class HitPointGroup : ComponentGroup<HitPointGroup, HitPointComponent, CharacterBaseComponent> { }

public class HitPointSystem : AGameSystem, IUpdateSystem
{
    public override void SetupEvents()
    {
        gameEvent.onFirstInitialize += Initialize;
    }

    public void Initialize()
    {
        entityManager.Foreach<HitPointGroup>((group) =>
        {
            group.data2.hitPoint = group.data2.hitPointMax;
        });
    }

    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<HitPointGroup>(FollowPlayer);
    }

    private void FollowPlayer(HitPointGroup group)
    {
        // group.data1.hitPointText.gameObject.transform.position = gameStat.playerGroup.entity.transform.position + group.data1.UIPositionOffset;
        group.data1.hitPointText.text = group.data2.hitPoint.ToString() + " / " + group.data2.hitPointMax.ToString();
    }
}
