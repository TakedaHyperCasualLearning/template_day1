using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{

    public abstract class ComponentGroup
    {
        public Entity entity;
        public bool isActivated = true;
        public abstract long GetTargetBits();
        public abstract void Init(Entity entity);
        public ComponentGroup CloneModel(Entity entity)
        {
            ComponentGroup instance = CreateInstance(entity);
            instance.Init(entity);
            return instance;
        }
        public abstract ComponentGroup CreateInstance(Entity entity);
    }

    public class ComponentGroup<T, T1> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
    {
        public T1 data1;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
        }
    }


    public class ComponentGroup<T, T1, T2> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
    {
        public T1 data1;
        public T2 data2;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
        }
    }

    public class ComponentGroup<T, T1, T2, T3> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
            data3 = entity.GetComponent<T3>();
        }
    }

    public class ComponentGroup<T, T1, T2, T3, T4> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
            data3 = entity.GetComponent<T3>();
            data4 = entity.GetComponent<T4>();
        }
    }


    public class ComponentGroup<T, T1, T2, T3, T4, T5> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;
        public T5 data5;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
            data3 = entity.GetComponent<T3>();
            data4 = entity.GetComponent<T4>();
            data5 = entity.GetComponent<T5>();
        }
    }

    public class ComponentGroup<T, T1, T2, T3, T4, T5, T6> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
    {
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;
        public T5 data5;
        public T6 data6;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
            data3 = entity.GetComponent<T3>();
            data4 = entity.GetComponent<T4>();
            data5 = entity.GetComponent<T5>();
            data6 = entity.GetComponent<T6>();
        }
    }

    public class ComponentGroup<T, T1, T2, T3, T4, T5, T6, T7> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 :IComponent
    {
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;
        public T5 data5;
        public T6 data6;
        public T7 data7;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
            data3 = entity.GetComponent<T3>();
            data4 = entity.GetComponent<T4>();
            data5 = entity.GetComponent<T5>();
            data6 = entity.GetComponent<T6>();
            data7 = entity.GetComponent<T7>();
        }
    }

    public class ComponentGroup<T, T1, T2, T3, T4, T5, T6, T7, T8> : ComponentGroup
        where T : ComponentGroup, new()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent
    {
        public T1 data1;
        public T2 data2;
        public T3 data3;
        public T4 data4;
        public T5 data5;
        public T6 data6;
        public T7 data7;
        public T8 data8;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new T();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<T1>();
        }

        public override void Init(Entity entity)
        {
            data1 = entity.GetComponent<T1>();
            data2 = entity.GetComponent<T2>();
            data3 = entity.GetComponent<T3>();
            data4 = entity.GetComponent<T4>();
            data5 = entity.GetComponent<T5>();
            data6 = entity.GetComponent<T6>();
            data7 = entity.GetComponent<T7>();
            data8 = entity.GetComponent<T8>();
        }
    }

}