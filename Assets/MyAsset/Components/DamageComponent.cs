using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class DamageComponent : IComponent
{
    public Entity owner { set; get; }
    public int damagePoint;
}
