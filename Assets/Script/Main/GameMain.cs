using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Donuts
{
    public class GameMain : MonoBehaviour
    {
        private MasterSystem masterSystem;
        protected GameMainUpdater updater;
        [SerializeField] protected FadeLayer fadeLayer;
        [SerializeField] protected float fadePeriod = 0.3f;
        [SerializeField] protected string sceneName = "GameScene";
        [SerializeField] private GameObject objectRoot;
        [SerializeField] private GameObject cameraObject;

        [SerializeField] private GameStat gameStat;
        private void Awake()
        {
            updater = gameObject.AddComponent<GameMainUpdater>();
            updater.enabled = false;
            fadeLayer.ForceOverlay();
            gameStat = new GameStat();
        }

        private IEnumerator Start()
        {
            yield return Singleton.Init();
            SetupMasterSystem();
            yield return fadeLayer.FadeIn(fadePeriod);
            FinishedLoading();

            for (int i = 0; i < objectRoot.transform.childCount; i++)
            {
                EntityComponent entity = objectRoot.transform.GetChild(i).GetComponent<EntityComponent>();
                if (entity == null) continue;
                masterSystem.gameEvent.onSpawnedEntity(entity.ToEntity());
            }
            masterSystem.gameEvent.onSpawnedEntity(cameraObject.GetComponent<EntityComponent>().ToEntity());

            Debug.Log("GameMain Start");
            masterSystem.gameEvent.onFirstInitialize?.Invoke();
        }

        private void SetupMasterSystem()
        {
            masterSystem = new MasterSystem(gameStat,
            new TouchSystem(),
            new PoolSystem(),
            new LifeTimeSystem(),

            new PlayerInputSystem(),
            new CharacterMoveSystem(),
            new PlayerAttackSystem(),
            new HitPointSystem(),

            new BulletMoveSystem(),
            new BulletHitSystem(),

            new EnemySpawnSystem(),
            new EnemyMoveSystem(),

            new DamageSystem(),

            new CameraMoveSystem(),

            new GameRuleSystem()
            );
        }

        private void FinishedLoading()
        {
            updater.masterSystem = masterSystem;
            updater.enabled = true;//checkout updater on update for main loop
        }

        protected void Reload()
        {
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return fadeLayer.FadeOut(fadePeriod);
            SceneManager.LoadScene(sceneName);
        }
    }
}