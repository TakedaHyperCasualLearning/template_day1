using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Donuts
{
    public partial class Singleton
    {
        public static Singleton instance;
        
        public ResourcesData resourceData;
        public SaveData saveData;

        public const string ResourceDataPath = "ResourcesData";
        public const string SaveDataPath = "GameSave";

        public static IEnumerator Init()
        {
            if (instance == null)
            {
                Application.targetFrameRate = 60;
                instance = new Singleton();
                ECSDefinition.Init();

                yield return instance.Preload();

                instance.LoadSave();
                instance.Initialized();

            }
            instance.Reset();
            
        }

        public IEnumerator Preload()
        {
            ResourceRequest request = Resources.LoadAsync<ResourcesData>(ResourceDataPath);
            if (request.isDone == false)
            {
                yield return null;
            }
            resourceData = request.asset as ResourcesData;
        }

        partial void Initialized();
        partial void Reset();

        private void LoadSave()
        {
            if (PlayerPrefs.HasKey(SaveDataPath) == false)
            {
                saveData = new SaveData();
                return;
            }

            string json = PlayerPrefs.GetString(SaveDataPath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SaveDataPath, json);
        }
    }

}