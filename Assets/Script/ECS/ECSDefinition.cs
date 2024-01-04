using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace Donuts
{
    internal static class ECSDefinition
    {
        public static int TypeNumbers = 0;
        private static Dictionary<Type, int> entityIdTable = new Dictionary<Type, int>();
        private static Dictionary<Type, long> entityBitTable = new Dictionary<Type, long>();
        private static bool initialized = false;

        private static Dictionary<Type, int> groupIdSlotTable = new Dictionary<Type, int>();//chuck type
        public static int groupNumbers = 0;
        internal static void Init()
        {
            if (initialized)
            {
                return;
            }
            initialized = true;
            SetupEntityIDTables();
            SetupComponentGroup();
        }

        private static void SetupEntityIDTables()
        {
            entityIdTable.Clear();
            entityBitTable.Clear();
            var parentType = typeof(IComponent);
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => parentType.IsAssignableFrom(p)).ToArray();


            int numbers = types.Length;
            TypeNumbers = 0;

            int currentID = 0;
            for (int i = 0; i < numbers; i++)
            {
                if (types[i] == parentType)
                {
                    continue;
                }
                entityIdTable.Add(types[i], currentID);
                entityBitTable.Add(types[i], (long)1 << currentID);
                currentID++;
                TypeNumbers++;
            }
        }


        private static void SetupComponentGroup()
        {
            var parentType = typeof(ComponentGroup);
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => parentType.IsAssignableFrom(p)).ToArray();
            int length = types.Length;

            groupIdSlotTable.Clear();
            int idCounter = 0;
            for (int i = 0; i < length; i++)
            {
                if (types[i] == parentType || types[i].IsGenericType)
                {
                    continue;
                }
                groupIdSlotTable.Add(types[i], idCounter);
                idCounter++;
            }
            groupNumbers = groupIdSlotTable.Count;// exclude original wrapper typetype;
        }

        internal static int GetComponentId<T>() where T : IComponent
        {
            return entityIdTable[typeof(T)];
        }

        internal static int GetComponentId(Type type)
        {
            return entityIdTable[type];
        }

        internal static long GetComponentBit<T>() where T : IComponent
        {
            return entityBitTable[typeof(T)];
        }

        internal static long GetComponentBit(Type t)
        {
            return entityBitTable[t];
        }

        internal static int GetComponentGroupIDSlot(Type type)//type must be componentgroup
        {
            return groupIdSlotTable[type];
        }
    }
}