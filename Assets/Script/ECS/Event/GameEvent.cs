using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Donuts
{
    public partial class GameEvent
    {
        public Action<Entity> onSpawnedEntity;
        public Action<Entity> onRemovedEntity;
        public Action onFinishedPreload;
        public Action<GameObject> onRemoveGameObject;
        public Action onFirstInitialize;

    }
}