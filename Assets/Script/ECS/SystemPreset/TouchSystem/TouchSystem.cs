using System.Diagnostics;
using UnityEngine;
using System;
namespace Donuts
{
    public partial class GameEvent
    {
        public Action<bool> onEnableTounch;
        public Action<Vector3> onOneTouchBegin;
        public Action<Vector3> onOneTouchMove;
        public Action<Vector3> onOneTouchStayed;
        public Action<Vector3> onOneTouchEnded;
        public Action<Vector3> onDoubleTouch;
        public Action<Vector3> onLongTouchBegin;
        public Action<Vector3> onOneTouchCanceled;

    }

    public class TouchSystem : AGameSystem, IUpdateSystem
    {
        // タッチ情報
        private TouchInfo info;
        private TouchInfo info2;
        // タッチ座標
        private Vector3 touchPos;
        private Vector3 touchPos2;

        // 1フレーム前のタッチ座標
        private Vector3 prevTouchPos;
        private Vector3 prevTouchPos2;

        // タッチ移動差分ベクトル
        private Vector3 deltaPos;
        private Vector3 deltaPos2;

        // ピンチの量取得
        private float betweenMag;
        private float prevBetweenMag;
        public float PinchDelta = 0;

        // ダブルタップ判定
        private bool isDoubleTapCheck;
        private float doubleTapCheckSec = 0;
        private const float DoubleTapSecond = 0.3f;

        // 長押し判定
        private bool isLongTouchCheck;
        private float longTouchCheckSec = 0;
        private const float LongTouchSecond = 0.78f;
        private const float LongTouchPermitDelta = 3.0f;

        // 判定するか
        public bool isEnabled = true;



        //initiation
        public TouchSystem()
        {
            
            OnEnableTouch(true);
        }

        public override void SetupEvents()
        {
            gameEvent.onEnableTounch += OnEnableTouch;
        }


        private void OnEnableTouch(bool _enable)
        {
            isEnabled = _enable;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!isEnabled) return;

            // ピンチの差分はupdate開始タイミングでは0にセットしておく
            PinchDelta = 0;
            // ピンチをエディタでデバッグする
            checkPinchEditor();

            // ダブルタップ判定用処理
            if (isDoubleTapCheck) doubleTapCheckSec += deltaTime;
            if (isDoubleTapCheck && doubleTapCheckSec >= DoubleTapSecond) isDoubleTapCheck = false;

            // ノータッチなら処理終了
            if (MultiPlatformTouchUtils.touchCount == 0) return;

            // 一本目のタッチ情報
            info = MultiPlatformTouchUtils.GetTouchWithGUI(0);
            touchPos = MultiPlatformTouchUtils.GetTouchPosition(0);

            if (info == TouchInfo.Began)
            {
                OnOneTouchBegan(touchPos);
                checkDoubleTap();
            }
            if (info == TouchInfo.Moved) { OnOneTouchMoved(touchPos); }
            if (info == TouchInfo.Stationary) { OnOneTouchStayed(touchPos); }
            if (info == TouchInfo.Ended) { OnOneTouchEnded(touchPos); }
            if (info == TouchInfo.Canceled) { OnOneTouchCanceled(touchPos); }

            // シングルタッチの移動差分取得
            checkDeltaPos(ref info, ref deltaPos, ref touchPos, ref prevTouchPos);

            // シングルタッチの長押しチェック
            checkLongTouch(deltaTime);

            // 2本タッチ以外は処理終了
            if (MultiPlatformTouchUtils.touchCount == 2)
            {

                // 二本目のタッチ情報
                info2 = MultiPlatformTouchUtils.GetTouchWithGUI(1);
                touchPos2 = MultiPlatformTouchUtils.GetTouchPosition(1);
                checkDeltaPos(ref info2, ref deltaPos2, ref touchPos2, ref prevTouchPos2);

                checkPinch();
            }

            prevPosUpdate(ref touchPos, ref prevTouchPos);
            prevPosUpdate(ref touchPos2, ref prevTouchPos2);
        }

        private void checkDoubleTap()
        {
            // ダブルタップ監視していない状態で一回目タップなら判定開始
            if (!isDoubleTapCheck)
            {
                isDoubleTapCheck = true;
                doubleTapCheckSec = 0;
            }
            // ダブルタップ成立
            else if (isDoubleTapCheck && doubleTapCheckSec < DoubleTapSecond)
            {
                isDoubleTapCheck = false;
                OnDoubleTap(touchPos);
            }
        }

        private void checkDeltaPos(ref TouchInfo tInfo, ref Vector3 dPos, ref Vector3 tPos, ref Vector3 pPos)
        {
            if (tInfo == TouchInfo.Moved) dPos = tPos - pPos;
            else dPos = Vector3.zero;
        }

        private void prevPosUpdate(ref Vector3 tPos, ref Vector3 pPos)
        {
            pPos = tPos;
        }

        private void checkLongTouch(float deltaTime)
        {

            // タッチ開始で判定開始
            if (info == TouchInfo.Began)
            {
                isLongTouchCheck = true;
                longTouchCheckSec = 0.0f;
                return;
            }

            if (!isLongTouchCheck) return;

            // 一応
            if (info == TouchInfo.Ended || info == TouchInfo.Canceled)
            {
                isLongTouchCheck = false;
                return;
            }

            // 移動差分が大きすぎなら判定false
            if (deltaPos.magnitude > LongTouchPermitDelta)
            {
                isLongTouchCheck = false;
                return;
            }

            // 静止 = 判定時間加算
            if (info == TouchInfo.Stationary)
            {
                longTouchCheckSec += deltaTime;
            }
            // 移動かつ移動差分が小さい = 判定時間加算
            else if (info == TouchInfo.Moved && deltaPos.magnitude < LongTouchPermitDelta)
            {
                longTouchCheckSec += deltaTime;
            }

            // 成立
            if (longTouchCheckSec > LongTouchSecond)
            {
                isLongTouchCheck = false;
                OnLongTouchBegin(touchPos);
            }
        }

        private void checkPinch()
        {
            if (info == TouchInfo.Began || info2 == TouchInfo.Began) return;
            if (info == TouchInfo.None || info2 == TouchInfo.None) return;

            // 各フレームのタッチ間の距離の大きさをもとめます
            betweenMag = (touchPos - touchPos2).magnitude;
            prevBetweenMag = (prevTouchPos - prevTouchPos2).magnitude;
            // 各フレーム間の距離の差をもとめます
            PinchDelta = betweenMag - prevBetweenMag;
        }

        [Conditional("UNITY_EDITOR")]
        private void checkPinchEditor()
        {
            if (Input.GetKey(KeyCode.UpArrow)) PinchDelta = 10;
            else if (Input.GetKey(KeyCode.DownArrow)) PinchDelta = -10;
        }

        protected void OnOneTouchBegan(Vector3 touchPos) { gameEvent.onOneTouchBegin?.Invoke(touchPos); }

     
        protected virtual void OnOneTouchMoved(Vector3 touchPos) { gameEvent.onOneTouchMove?.Invoke(touchPos); }


        protected virtual void OnOneTouchStayed(Vector3 touchPos) { gameEvent.onOneTouchStayed?.Invoke(touchPos); }


        protected virtual void OnOneTouchEnded(Vector3 touchPos) { gameEvent.onOneTouchEnded?.Invoke(touchPos); }

        protected virtual void OnOneTouchCanceled(Vector3 touchPos) { gameEvent.onOneTouchCanceled?.Invoke(touchPos); }

        protected virtual void OnDoubleTap(Vector3 touchPos) { gameEvent.onDoubleTouch?.Invoke(touchPos); }

        protected virtual void OnLongTouchBegin(Vector3 touchPos) { gameEvent.onLongTouchBegin?.Invoke(touchPos); }

        protected (bool isHit, GameObject hitObj) getHitInfo(Vector3 touchPos, float dist = Mathf.Infinity)
        {
            RaycastHit hit;
            bool ishit = isHit(touchPos, out hit);
            GameObject hitObj = null;
            if (ishit) hitObj = hit.collider.gameObject;
            return (ishit, hitObj);
        }

        protected bool isHit(Vector3 touchPos, out RaycastHit hit, float dist = Mathf.Infinity)
        {
            // メインカメラ上のタッチ位置からRayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            // レイキャスト
            return Physics.Raycast(ray, out hit, dist);
        }
    }

}
