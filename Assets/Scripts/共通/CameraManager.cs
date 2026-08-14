using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _firstPersonCamera;   // 一人称視点の参照
    [SerializeField] private CinemachineVirtualCamera _thirdPersonCamera;   // 三人称視点の参照

    [SerializeField] private GameObject _trueImg;                           // 撮影判定true
    [SerializeField] private GameObject _falseImg;                          // 撮影判定false
    [SerializeField] private GameObject _actionUIButton;                    // 撮影用UIButton
    [SerializeField] private GameObject _moveUIButton;                      // 移動用UIButton

    public bool fpsFlg;                                                      // 一人称フラグ
    public bool rayCastFlg = false;                                         // rayCastフラグ

    public float distance = 7.0f;                                           // 検出可能な距離

    void Start()
    {
        // 最初は三人称から
        SetThirdPersonCamera();

        // 撮影判定イメージの設定
        _trueImg.SetActive(false);
        _falseImg.SetActive(true);
    }

    void Update()
    {
        // 一人称判定
        if (fpsFlg)
        {

            //カメラ内外判定
            IsVisibleByCamera();

            // 撮影対象のレイキャスト当たり判定
            if (rayCastFlg)
            {
                // 撮影判定を〇とする
                _trueImg.SetActive(true);
                _falseImg.SetActive(false);
            }
            else
            {
                // 撮影判定を×とする
                _trueImg.SetActive(false);
                _falseImg.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 視点の切り替えを実行する
    /// </summary>
    [ContextMenu("SwitchCamera")]
    public void SwitchCamera()
    {
        if (fpsFlg)
        {
            // 三人称モード
            SetThirdPersonCamera();
        }
        else
        {
            // 一人称モード
            SetFirstPersonCamera();
        }
    }

    /// <summary>
    /// 一人称視点に切り替える
    /// </summary>
    private void SetFirstPersonCamera()
    {
        // カメラのPriorityを変更し一人称カメラ優先とする
        _firstPersonCamera.Priority = 10;
        _thirdPersonCamera.Priority = 0;

        // アクションボタン活性、一人称フラグ活性
        _actionUIButton.SetActive(true);
        fpsFlg = true;
    }

    /// <summary>
    /// 三人称視点に切り替える
    /// </summary>
    private void SetThirdPersonCamera()
    {
        // カメラのPriorityを変更し三人称カメラ優先とする
        _firstPersonCamera.Priority = 0;
        _thirdPersonCamera.Priority = 10;

        // アクションボタン非活性、一人称フラグ非活性
        _actionUIButton.SetActive(false);
        fpsFlg = false;
    }

    /// <summary>
    /// カメラ撮影判定(レイキャストによる対象物判定)
    /// </summary>
    public void IsVisibleByCamera()
    {
        // 一人称の場合に判定処理を行う
        // RayCastによるオブジェクト判定
        // Rayはカメラの位置からとばす
        var rayStartPosition = _firstPersonCamera.transform.position;
        // Rayはカメラが向いてる方向にとばす
        var rayDirection = _firstPersonCamera.transform.forward.normalized;

        // Hitしたオブジェクト格納用
        RaycastHit raycastHit;

        // Rayを飛ばす（out raycastHit でHitしたオブジェクトを取得する）
        var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);

        // Debug.DrawRay (Vector3 start(rayを開始する位置), Vector3 dir(rayの方向と長さ), Color color(ラインの色));
        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        // レイキャストで何かを確認かつそれが撮影対象であればレイキャストフラグを立てる
        if (isHit && raycastHit.collider.gameObject.CompareTag("CaptureTarget"))
        {
            rayCastFlg = true;
        }
        else
        {
            rayCastFlg = false;
        }
    }
}
