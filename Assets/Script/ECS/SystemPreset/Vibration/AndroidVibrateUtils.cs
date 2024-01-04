using UnityEngine;
using System.Collections;

public static class AndroidVibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    /// <summary>
    /// バイブレーション
    /// </summary>
    /// <param name="milliseconds"> Vibration Length. </param>
    public static void Vibrate(long milliseconds)
    {
        if (isAndroid()) vibrator.Call("vibrate", milliseconds);
        else Handheld.Vibrate();
    }

    /// <summary>
    /// パターンバイブレーション
    /// </summary>
    /// <param name="pattern"> Vibration pattern. { "Interval", "Vibration Length", "Interval", "Vibration Length", ...}</param>
    /// <param name="repeat"> -1: one time, other: "vibrator.cancel()", will stop. </param>
    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid()) vibrator.Call("vibrate", pattern, repeat);
        else Handheld.Vibrate();
    }

    /// <summary>
    /// バイブレーションキャンセル
    /// </summary>
    public static void Cancel()
    {
        if (isAndroid()) vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	    return true;
#else
        return false;
#endif
    }
}