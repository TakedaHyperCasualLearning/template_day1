using UnityEngine;

namespace CommonDonutsHC {
  namespace UI {

    /****************************************************************/
    /// <summary>
    /// セーフエリア対応コンポーネント
    /// このコンポーネントをつけた階層はセーフエリア対応となる
    /// </summary>
    /****************************************************************/
    public class SafeAreaFixer : MonoBehaviour {

      // 調整対象レクトトランスフォーム
      private RectTransform targetRectTran;

      // スクリーンサイズ
      private Vector2Int screenSize;

      // セーフエリア
      private Rect safeAreaRect;

      /****************************************************************/
      /// <summary>
      /// 起動時処理
      /// </summary>
      /****************************************************************/
      void Awake() {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
          ReferenceSafeAreaRect();
          FixToSafeArea();
        }
      }

      /****************************************************************/
      /// <summary>
      /// セーフエリアレクトを参照
      /// </summary>
      /****************************************************************/
      public void ReferenceSafeAreaRect() {
        safeAreaRect = Screen.safeArea;
      }

      /****************************************************************/
      /// <summary>
      /// セーフエリア対応処理
      /// </summary>
      /****************************************************************/
      public void FixToSafeArea() {
        // 対象レクトトランスフォームを参照
        targetRectTran = GetComponent<RectTransform>();

        // CanvasのAnchorズレ問題解消
        var display = Display.displays[0];
        screenSize = new Vector2Int(display.systemWidth, display.systemHeight);

        // 調整
        var anchorMin = safeAreaRect.position;
        var anchorMax = safeAreaRect.position + safeAreaRect.size;
        anchorMin.x /= screenSize.x;
        anchorMin.y /= screenSize.y;
        anchorMax.x /= screenSize.x;
        anchorMax.y /= screenSize.y;
        targetRectTran.anchorMin = anchorMin;
        targetRectTran.anchorMax = anchorMax;

        // 本来はUIサイズもスケールする必要があるが、誤差の範囲とした
      }

      #region Debug on Editor
      #if UNITY_EDITOR

      // 縦持ちのみ考慮

      // シミュレート機種
      private enum SimulateType {
        None = -1,
        iPhoneX,
        iPhoneXs,
        iPhoneXr,
        iPhoneXsMax
      }

      // 論理尺度(シミュレートタイプと合わせる)
      private readonly Vector2[] ScreenPointSizes = new Vector2[] {
        new Vector2(375, 812), // iPhoneX
        new Vector2(375, 812), // iPhoneXs
        new Vector2(414, 896), // iPhoneXr
        new Vector2(414, 896)  // iPhoneXsMax
      };

      // マージン幅
      private const float MerginTop = 44f;
      private const float MerginBtm = 34f;

      // シミュレートタイプ
      [SerializeField] private SimulateType simulateType;

      /****************************************************************/
      /// <summary>
      /// Setups the safe area rect for simulate.
      /// </summary>
      /****************************************************************/
      public void setupSafeAreaForSimulate() {
        if (simulateType == SimulateType.None) return;

        Debug.Log(string.Format("シミュレート:{0}", simulateType));

        var screenPointSize = ScreenPointSizes[(int)simulateType];

        var originOffsetX = 0.0f;
        var originOffsetY = safeAreaRect.size.y * (MerginBtm / screenPointSize.y);
        var originOffset = new Vector2(originOffsetX, originOffsetY);

        var sizeOffsetX = 0.0f;
        var sizeOffsetY = safeAreaRect.size.y * ((MerginTop + MerginBtm) / screenPointSize.y);
        var sizeOffset = new Vector2(sizeOffsetX, sizeOffsetY);

        safeAreaRect.position = safeAreaRect.position + originOffset;
        safeAreaRect.size = safeAreaRect.size - sizeOffset;
      }
      #endif
      #endregion
    }
  }
}
/********************************** END **********************************/
