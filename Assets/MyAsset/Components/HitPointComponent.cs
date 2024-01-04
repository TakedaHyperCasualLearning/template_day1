using System.Collections;
using System.Collections.Generic;
using Donuts;
using TMPro;
using UnityEngine;

public class HitPointComponent : IComponent
{
    public Entity owner { set; get; }
    public TextMeshPro hitPointText;
    public Vector3 UIPositionOffset;
}
