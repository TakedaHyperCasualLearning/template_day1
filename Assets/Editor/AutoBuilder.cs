using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoBuilder
{
  // Please Insert Facebook AppID.
  private const string FACEBOOK_APPID = "559404105147477";
  
  //************************************************************************************************
  /// <summary>
  /// BuildPlayerAndroid.shが叩くビルドコマンド
  /// </summary>
  //************************************************************************************************
  public static void BuildPlayerAndroid()
  {
    //if (Facebook.Unity.Settings.FacebookSettings.AppId != FACEBOOK_APPID) throw new Exception("Facebook AppID don't match!!!!!");
    
    PlayerSettings.Android.keystoreName = "hc.keystore";
    PlayerSettings.Android.keystorePass = "DonutsHC123%";
    PlayerSettings.Android.keyaliasName = "hc";
    PlayerSettings.Android.keyaliasPass = "DonutsHC123%";

    var buildPlayerOptions = new BuildPlayerOptions();
    buildPlayerOptions.scenes = GetScenePaths();
    buildPlayerOptions.locationPathName = "Builds/" + Application.productName + ".aab";
    buildPlayerOptions.target = BuildTarget.Android;
    buildPlayerOptions.options = BuildOptions.None;
    EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
    EditorUserBuildSettings.buildAppBundle = true;
    var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
    if (buildReport.summary.result == BuildResult.Failed) throw new Exception("Build Android player failed");
  }

  //************************************************************************************************
  /// <summary>
  /// BuildPlayeriOS.shが叩くビルドコマンド
  /// </summary>
  //************************************************************************************************
  public static void BuildPlayeriOS()
  {
    //if (Facebook.Unity.Settings.FacebookSettings.AppId != FACEBOOK_APPID) throw new Exception("Facebook AppID don't match!!!!!");

    var buildPlayerOptions = new BuildPlayerOptions();
    buildPlayerOptions.scenes = GetScenePaths();
    buildPlayerOptions.locationPathName = "Builds/";
    buildPlayerOptions.target = BuildTarget.iOS;
    buildPlayerOptions.options = BuildOptions.None;
    var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
    if (buildReport.summary.result == BuildResult.Failed) throw new Exception("Build iOS player failed");
  }

  //************************************************************************************************
  /// <summary>
  /// SceneInBuildにAddしたシーンを取得します
  /// </summary>
  //************************************************************************************************
  static string[] GetScenePaths()
  {
    var scenes = new List<string>();

    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
    {
      if (EditorBuildSettings.scenes[i].path != "") scenes.Add(EditorBuildSettings.scenes[i].path);
    }

    return scenes.ToArray();
  }
}
