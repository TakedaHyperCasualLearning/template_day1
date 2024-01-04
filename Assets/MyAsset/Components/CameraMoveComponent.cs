using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class CameraMoveComponent : IComponent
{
    public Entity owner { set; get; }
    public Vector3 positionOffset;
}
