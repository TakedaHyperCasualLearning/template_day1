using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class CameraMoveGroup : ComponentGroup<CameraMoveGroup, CameraMoveComponent> { }

public class CameraMoveSystem : AGameSystem, IUpdateSystem
{
    public void OnUpdate(float deltaTime)
    {
        entityManager.Foreach<CameraMoveGroup>(MoveCamera);
    }

    private void MoveCamera(CameraMoveGroup group)
    {
        group.entity.transform.position = gameStat.playerGroup.entity.transform.position + group.data1.positionOffset;
    }

}
