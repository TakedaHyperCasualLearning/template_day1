using System.Collections.Generic;

namespace Donuts
{
    public class MasterSystem
    {
        private AGameSystem[] systems;
        private IPreUpdateSystem[] longPreUpdateSystems;
        private IUpdateSystem[] updateSystems;
        private IPostUpdateSystem[] longUpdateSystems;
        private int systemsCount = 0;
        private int updateSystemCount = 0;
        private int longPreUpdateNumbers = 0;
        private int longUpdateNumbers = 0;
        public EntityManager entityManager;
        public GameEvent gameEvent;
        public GameFunction gameFunction;
        public GameStat gameStat;
        public MasterSystem(GameStat _gameStat, params AGameSystem[] _systems)
        {

            gameEvent = new GameEvent();
            gameFunction = new GameFunction();
            entityManager = new EntityManager(gameEvent);
            gameStat = _gameStat;
            systems = _systems;
            systemsCount = systems.Length;

            List<IUpdateSystem> updateList = new List<IUpdateSystem>();
            List<ILateUpdateSystem> lateUpdateList = new List<ILateUpdateSystem>();
            List<IPreUpdateSystem> preUpdateList = new List<IPreUpdateSystem>();
            List<IPostUpdateSystem> longUpdateList = new List<IPostUpdateSystem>();
            for (int i = 0; i < systemsCount; i++)
            {
                systems[i].masterSystem = this;
                systems[i].gameEvent = gameEvent;
                systems[i].gameFunction = gameFunction;
                systems[i].gameEvent = gameEvent;
                systems[i].entityManager = entityManager;
                systems[i].gameStat = gameStat;
                if (systems[i] is IUpdateSystem)
                {
                    updateList.Add(systems[i] as IUpdateSystem);
                }
                if (systems[i] is IPreUpdateSystem)
                {
                    preUpdateList.Add(systems[i] as IPreUpdateSystem);
                }
                if (systems[i] is IPostUpdateSystem)
                {
                    longUpdateList.Add(systems[i] as IPostUpdateSystem);
                }
                if (systems[i] is ILateUpdateSystem)
                {
                    lateUpdateList.Add(systems[i] as ILateUpdateSystem);
                }
                systems[i].SetupEvents();
            }

            updateSystems = updateList.ToArray();
            updateSystemCount = updateSystems.Length;
            longPreUpdateSystems = preUpdateList.ToArray();
            longPreUpdateNumbers = longPreUpdateSystems.Length;
            longUpdateSystems = longUpdateList.ToArray();
            longUpdateNumbers = longUpdateSystems.Length;
        }


        public void AddSystem(AGameSystem gameSystem)
        {
            List<AGameSystem> systemList = new List<AGameSystem>(systems);
            systemList.Add(gameSystem);
            systems = systemList.ToArray();
            systemsCount = systems.Length;
            if (gameSystem is IUpdateSystem)
            {
                List<IUpdateSystem> updateList = new List<IUpdateSystem>(updateSystems);
                updateList.Add(gameSystem as IUpdateSystem);
                updateSystems = updateList.ToArray();
                updateSystemCount = updateSystems.Length;
            }
        }

        public void RemoveSystem(AGameSystem gameSystem)
        {
            List<AGameSystem> systemList = new List<AGameSystem>(systems);
            systemList.Remove(gameSystem);
            systems = systemList.ToArray();
            systemsCount = systems.Length;
            if (gameSystem is IUpdateSystem)
            {
                List<IUpdateSystem> updateList = new List<IUpdateSystem>(updateSystems);
                updateList.Remove(gameSystem as IUpdateSystem);
                updateSystems = updateList.ToArray();
                updateSystemCount = updateSystems.Length;
            }
        }

        #region event

        public void OnUpdate(float deltaTime)
        {

            for (int i = 0; i < longPreUpdateNumbers; i++)
            {
                longPreUpdateSystems[i].StartPreUpdate();
                while (longPreUpdateSystems[i].IsFinishedPreUpdate() == false)
                {
                    longPreUpdateSystems[i].OnPreUpdate(deltaTime);
                }
            }
            for (int i = 0; i < updateSystemCount; i++)
            {
                updateSystems[i].OnUpdate(deltaTime);
            }

            for (int i = 0; i < longUpdateNumbers; i++)
            {
                longUpdateSystems[i].StarPostUpdate();
                while (longUpdateSystems[i].IsFinishedPostUpdate() == false)
                {
                    longUpdateSystems[i].OnPostUpdate(deltaTime);
                }
            }

        }

        public void Destructor()
        {
            DestroyAllSystems();
            entityManager.OnDestroy();
            gameFunction = null;
            gameEvent = null;
        }

        private void DestroyAllSystems()
        {
            int length = systems.Length;
            for (int i = 0; i < length; i++)
            {
                systems[i].OnDestroy();
            }
            systems = null;
            updateSystems = null;
        }

        #endregion
    }
}