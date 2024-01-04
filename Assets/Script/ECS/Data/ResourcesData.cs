using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Donuts
{
    [CreateAssetMenu(fileName = "ResourcesData", menuName = "ECS/ResourcesData")]
    public partial class ResourcesData : ScriptableObject
    {
        public GameObject enemyPrefab;
        public GameObject bulletPrefab;
    }
}