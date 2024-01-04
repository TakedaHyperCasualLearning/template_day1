using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System.IO;

public static class PostProcessBuild
{
  private const string ATT_FRAMEWORK = "AppTrackingTransparency.framework";
  private const string AS_FRAMEWORK = "AdSupport.framework";

  [PostProcessBuild]
  public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
  {
    // iOS以外は未処理
    if (buildTarget != BuildTarget.iOS) return;

    Debug.Log("Started Running HyperCasual PostProcess Build");

    // info.plistを編集してurlスキーマを入れる　
    var plistPath = Path.Combine(path, "Info.plist");
    var plist = new PlistDocument();
    plist.ReadFromFile(plistPath);
    
    // IDFA用の承認ポップアップ表示用
    plist.root.SetString("NSUserTrackingUsageDescription", "This identifier will be used to deliver personalized ads to you.");
    
    // AdmobID(アプリごとに変更する必要あり)
    // plist.root.SetString("GADApplicationIdentifier", "ca-app-pub-9390168136487188~1729556431");
    
    // IronSourceのSKAdNetworkID登録
    var array = plist.root.CreateArray("SKAdNetworkItems");
    array.AddDict().SetString("SKAdNetworkIdentifier", "su67r6k2v3.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "4pfyvq9l8r.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "cstr6suwn9.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "ludvb6z3bs.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "v9wttpbfk9.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "n38lu8286q.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "9g2aggbj52.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "nu4557a4je.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "wzmmz9fp6w.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "v4nxqhlyqp.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "22mmun2rn5.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "238da6jt44.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "424m5254lk.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "ecpz2srf59.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "4dzt52r2t5.skadnetwork");
    array.AddDict().SetString("SKAdNetworkIdentifier", "gta9lk7p23.skadnetwork");

    // ガレンダー未使用示唆用
    // plist.root.SetString("NSCalendarsUsageDescription", "Advertisement would like to create a calendar event.");

    plist.WriteToFile(plistPath);

    // プロジェクトファイル
    string projPath = PBXProject.GetPBXProjectPath(path);
    // BuildSettingsをいじるためのオブジェクト
    var pbxProj = new PBXProject();
    // 読み込み
    pbxProj.ReadFromFile(projPath);
    // GUIDを取得する
    string target = pbxProj.GetUnityMainTargetGuid();
    pbxProj.AddFrameworkToProject(target, ATT_FRAMEWORK, true);
    pbxProj.AddFrameworkToProject(target, AS_FRAMEWORK, true);
    pbxProj.WriteToFile(projPath);

    // BuildSettingsを書き換える
    pbxProj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(TOOLCHAIN_DIR)/usr/lib/swift-5.0/$(PLATFORM_NAME)");
    pbxProj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(TOOLCHAIN_DIR)/usr/lib/swift/$(PLATFORM_NAME)");
    
    // PushNotificationsCaution回避用
    // pbxProj.AddCapability(target, PBXCapabilityType.PushNotifications);

    // 設定を保存
    File.WriteAllText(projPath, pbxProj.WriteToString());
    Debug.Log("Finished Running HyperCasual PostProcess Build");
  }
}