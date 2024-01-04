using System.Collections;
using System.Collections.Generic;
using Donuts;
using UnityEngine;

public class EnemySpawnComponent : IComponent
{
    public Entity owner { get; set; }

    public float spawnInterval = 0.0f;
    public float timer = 0.0f;
}
