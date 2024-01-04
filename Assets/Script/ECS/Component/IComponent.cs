using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Donuts
{
    public interface IComponent
    {
        Entity owner { get; set; }
    }
}