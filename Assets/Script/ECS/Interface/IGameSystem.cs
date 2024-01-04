using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Donuts
{
    public abstract class AGameSystem
    {
        public MasterSystem masterSystem;
        public GameEvent gameEvent;
        public GameFunction gameFunction;
        public EntityManager entityManager;
        public GameStat gameStat;

        public virtual void SetupEvents() { }
        public virtual void OnDestroy() { }
    }
}