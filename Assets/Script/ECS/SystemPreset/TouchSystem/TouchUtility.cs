using UnityEngine;

namespace Donuts
{

    public class MultiPlatformTouchUtils
    {
        private static Vector3 TouchPosition = Vector3.zero;
        private static Vector3 PreviousPosition = Vector3.zero;

        private static bool isObjCovering;

        public static int touchCount
        {
            get
            {
                if (Application.isEditor)
                {
                    if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return Input.touchCount;
                }
            }
        }

        public static TouchInfo GetTouch(int i)
        {
            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    return TouchInfo.Began;
                }
                if (Input.GetMouseButton(0))
                {
                    return TouchInfo.Moved;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    return TouchInfo.Ended;
                }
            }
            else
            {
                if (Input.touchCount >= i)
                {
                    return (TouchInfo)((int)Input.GetTouch(i).phase);
                }
            }
            return TouchInfo.None;
        }

        public static TouchInfo GetTouchWithGUI(int i)
        {
            // エディタ
            if (Application.isEditor)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    checkObjCovering();
                    if (isObjCovering) return TouchInfo.None;
                    return TouchInfo.Began;
                }
                if (Input.GetMouseButton(0))
                {
                    if (isObjCovering) return TouchInfo.None;
                    return TouchInfo.Moved;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (isObjCovering) return TouchInfo.None;
                    return TouchInfo.Ended;
                }
            }
            
            else
            {
                if (Input.touchCount >= i)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began) checkObjCovering(i);
                    if (isObjCovering) return TouchInfo.None;
                    return (TouchInfo)((int)Input.GetTouch(i).phase);
                }
            }
            return TouchInfo.None;
        }

        private static void checkObjCovering(int fingerId = -1)
        {
            
            bool isCovering = false;
            try
            {
                isCovering = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(fingerId);
            }
            catch
            {
                Debug.LogError("UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(fingerId) ERROR!!!");
                Debug.LogError("カバーオブジェクトを検知しない扱いでエラーハンドルします。");
                isCovering = false;
            }
            isObjCovering = isCovering;
        }

        public static Vector3 GetTouchPosition(int i)
        {
            if (Application.isEditor)
            {
                TouchInfo touch = MultiPlatformTouchUtils.GetTouch(i);
                if (touch != TouchInfo.None)
                {
                    PreviousPosition = Input.mousePosition;
                    return PreviousPosition;
                }
            }
            else
            {
                if (Input.touchCount >= i)
                {
                    Touch touch = Input.GetTouch(i);
                    TouchPosition.x = touch.position.x;
                    TouchPosition.y = touch.position.y;
                    return TouchPosition;
                }
            }
            return Vector3.zero;
        }
    }


    public enum TouchInfo
    {
        None = -1,
        Began = 0,
        Moved = 1,
        Stationary = 2,
        Ended = 3,
        Canceled = 4,
    }

}
