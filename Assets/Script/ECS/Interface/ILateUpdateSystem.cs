using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{
    public interface ILateUpdateSystem
    {
        void OnLateUpdate(float deltaTime);
    }
}