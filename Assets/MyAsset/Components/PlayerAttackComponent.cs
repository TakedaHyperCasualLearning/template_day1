using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class PlayerAttackComponent : IComponent
{
    public Entity owner { set; get; }
    public float attackPoint;
    public float interval;
    public float timer;
    public bool isCanShot;
}
