using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class ColliderComponent : IComponent
{
    public Entity owner { set; get; }
    public float radius;
}
