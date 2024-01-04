using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class CharacterBaseComponent : IComponent
{
    public Entity owner { get; set; }
    public int hitPointMax = 0;
    private int hitPoint = 0;
}
