using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
namespace Donuts
{
    public enum HapticsType
    {
        Tiny = 999,
        Light = 1519,
        Medium = 1520,
        Success = 1521
    }

    public partial class GameEvent
    {
        /// <summary> Haptics </summary> ///
        public Action<HapticsType> onVribrate;
    }

    public class HapticsSystem : AGameSystem
    {
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport ("__Internal")]
    static extern void _playSystemSound(int n);
    [DllImport ("__Internal")]
    static extern void _playSystemSelection();
#endif

        public override void SetupEvents()
        {
            gameEvent.onVribrate += Vibrate;
        }

        /// ************************************************************************
        /// <summary>
        /// iosのバイブレーション
        /// </summary>
        /// <param name="type"> Vibration Type.</param>
        /// ************************************************************************
        public void Vibrate(HapticsType type)
        {
            if (type == HapticsType.Tiny)
            {
                try
                {
#if UNITY_IOS && !UNITY_EDITOR
        _playSystemSelection();
#endif
                }
                catch
                {

                }
                return;
            }

#if UNITY_IOS && !UNITY_EDITOR
      _playSystemSound((int)type);
#elif UNITY_ANDROID && !UNITY_EDITOR
            var cls = new AndroidJavaClass("android.os.Build$VERSION");
            var apiLevel = cls.GetStatic<int>("SDK_INT");
            if (apiLevel >= 28) // AOS8.0以上
            {
                switch (type)
                {
                    case HapticsType.Tiny:
                        AndroidVibration.Vibrate(1);
                        break;
                    case HapticsType.Light:
                        AndroidVibration.Vibrate(40);
                        break;
                    case HapticsType.Medium:
                        AndroidVibration.Vibrate(60);
                        break;
                    case HapticsType.Success:
                        long[] pattern = { 0, 60, 50, 60 };
                        AndroidVibration.Vibrate(pattern, -1);
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case HapticsType.Tiny:
                        AndroidVibration.Vibrate(1);
                        break;
                    case HapticsType.Light:
                        AndroidVibration.Vibrate(10);
                        break;
                    case HapticsType.Medium:
                        AndroidVibration.Vibrate(20);
                        break;
                    case HapticsType.Success:
                        long[] pattern = { 0, 20, 50, 20 };
                        AndroidVibration.Vibrate(pattern, -1);
                        break;
                }
            }
#endif
        }
    }
}