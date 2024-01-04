using UnityEngine;
using UnityEditor;
namespace Donuts
{
    public class SaveDataEditor
    {
        [MenuItem("SaveData/Delete Save")]
        public static void DeleteSaveData()
        {
            PlayerPrefs.DeleteAll();
        }

    }
}