using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class BulletAttackComponent : IComponent
{
    public Entity owner { set; get; }
    public int attackPoint;
}
