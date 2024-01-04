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

        [SerializeField] private GameStat gameStat;
        private void Awake()
        {
            updater = gameObject.AddComponent<GameMainUpdater>();
            updater.enabled = false;
            fadeLayer.ForceOverlay();
        }

        private IEnumerator Start()
        {
            yield return Singleton.Init();
            SetupMasterSystem();
            yield return fadeLayer.FadeIn(fadePeriod);
            FinishedLoading();
        }

        private void SetupMasterSystem()
        {
            masterSystem = new MasterSystem(gameStat);
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