using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace Donuts
{
    public class AutoCreateResourcesData : MonoBehaviour
    {
        private const string directoryPath = "Assets/Resources";
        private const string fileName = "/ResourcesData.asset";
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CreateAssetWhenReady()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall -= CreateAssetWhenReady;
                EditorApplication.delayCall += CreateAssetWhenReady;
                return;
            }

            EditorApplication.delayCall += CreateAssetNow;
        }

        private static void CreateAssetNow()
        {
            if (Directory.Exists(directoryPath) == false)
            {
                //if it doesn't, create it
                Directory.CreateDirectory(directoryPath);
            }
            if (File.Exists(directoryPath + fileName) == false)
            {
                ResourcesData data = ScriptableObject.CreateInstance<ResourcesData>();
                AssetDatabase.CreateAsset(data, directoryPath + fileName);
                AssetDatabase.SaveAssets();

            }
        }
    }
}