using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class BulletMoveComponent : IComponent
{
    public Entity owner { set; get; }
    public Vector3 direction;
    public float speed;
}
