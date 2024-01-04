using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{
    public class GameMainUpdater : MonoBehaviour
    {
        [System.NonSerialized] public MasterSystem masterSystem;

        public void Update()
        {
            masterSystem.OnUpdate(Time.deltaTime);
        }
    }
}