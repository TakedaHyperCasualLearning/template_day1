using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{
    public interface IDrawGizmos
    {
        bool shouldDrawGizmos { get; set; }
        void OnDrawGizmos(GameObject gameObject);
    }

}