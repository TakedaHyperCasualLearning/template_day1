using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEditor.DeviceSimulation;
using UnityEngine;

public class PlayerInputGroup : ComponentGroup<PlayerInputGroup, PlayerInputComponent, CharacterMoveComponent> { }

public class PlayerInputSystem : AGameSystem, IUpdateSystem
{
    public override void SetupEvents()
    {
        masterSystem.gameEvent.onOneTouchMove += InputMove;
        masterSystem.gameEvent.onOneTouchEnded += InputChancel;
    }
    public void OnUpdate(float deltaTime)
    {
    }

    private void InputMove(Vector3 pos)
    {
        entityManager.Foreach<PlayerInputGroup, Vector3>(MoveCharacter, pos);
    }

    private void MoveCharacter(PlayerInputGroup group, Vector3 pos)
    {
        Vector3 playerPos = Camera.main.WorldToScreenPoint(group.entity.transform.position);
        Vector3 mousePos = pos;
        Vector3 direction = mousePos - playerPos;
        direction.z = 0.0f;
        group.data2.direction = new Vector3(direction.x, 0.0f, direction.y).normalized;

        Vector3 playerPoint = Camera.main.WorldToScreenPoint(group.entity.transform.position);
        Vector3 rotationDirection = Input.mousePosition - playerPoint;
        rotationDirection = rotationDirection.normalized;
        rotationDirection.z = 0.0f;
        group.data2.targetPosition = Camera.main.ScreenToWorldPoint(playerPoint + rotationDirection);
    }

    private void InputChancel(Vector3 pos)
    {
        entityManager.Foreach<PlayerInputGroup, Vector3>(ChancelCharacter, pos);
    }

    private void ChancelCharacter(PlayerInputGroup group, Vector3 pos)
    {
        group.data2.direction = Vector3.zero;
    }
}
