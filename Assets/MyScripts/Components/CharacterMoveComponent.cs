using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Donuts;

public class CharacterMoveComponent : IComponent
{
    public Entity owner { get; set; }
    public Vector3 direction;
    public float speed = 0.0f;
    public Vector3 targetPosition;
}