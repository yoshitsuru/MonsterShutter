using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Windows;
using StarterAssets;
using TMPro;

public class CameraManeger : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] public Camera _mainCamera;
    private bool _isFPS;            // FPSモード
    private bool _isTPS;            // TPSモード

    public float _lookSpeedAdjust = 1.0f;   // 感度調整

    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera _ThirdPersonCamera;
    [SerializeField] private CinemachineVirtualCamera _FirstPersonCamera;

    public bool _isClear;           // クリア判定
    public float distance = 6.0f;   // 検出可能な距離
    public int filmlimit = 10;      // 写真枚数制限
    private int _useFilm = 0;       // 使用したフィルム数
    public int _restFilm = 0;      // 残りフィルム数
    private float _forceTime;       // アクション可インターバル制御時間
    private bool _flagTime;         // アクション不可インターバルフラグ
    private bool _isHit;            // 撮影対象のオブジェクトに判定レーザーが触れているかどうか
    

    [Space(10)]
    [Tooltip("Shutter Flash Panel")]
    [SerializeField] public FlashAction _flashAction;

    [Space(10)]
    [Tooltip("Shutter Sound")]
    private AudioSource audioSource;
    public AudioClip _shutterSound; // シャッター音
    [Range(0, 1)] public float _shutterSoundVolume = 0.5f;

    
    public TextMeshProUGUI _textRestFilms;  // 残りフィルム数表示
    public GameObject _shutterButton;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        _ThirdPersonCamera.Priority = 10;
        _isTPS = true;
        _shutterButton.SetActive(false);

        _FirstPersonCamera.Priority = 0;
        _isFPS = false;
        _forceTime = 0.3f;

        _isClear = false;
    }

    private void Update()
    {
        // インターバル制御
        if (_forceTime <= 0)
        {
            _flagTime = false;
        }
        _forceTime -= Time.deltaTime;

        // フィルム枚数を計算
        CalcFilms();

        // FPS視点の時、カメラから判定用レーザーを照射
        if (_isFPS)
        {
            CheckTargetInCamera();
        }
        

    }

    /// <summary>
    /// デフォルトの3人称視点とカメラの1人称視点の切り替え
    /// </summary>
    public void CameraMode()
    {
        if (_isFPS)
        {
            // 三人称に切り替える
            SetThirdPersonCamera();
        }
        else if (_isTPS) 
        {
            // 一人称に切り替える
            SetFirstPersonCamera();
        }

    }

    /// <summary>
    /// 残りフィルム数の計算
    /// </summary>
    private void CalcFilms()
    {
        _restFilm = filmlimit - _useFilm;
        if (_restFilm > 0)
        {
            _textRestFilms.text = $"フィルム：{_restFilm}枚";
            _textRestFilms.color = Color.green;
        }
        else if (_restFilm <= 0)
        {
            _textRestFilms.text = "EMPTY";
            _textRestFilms.color = Color.red;
        }
    }

    /// <summary>
    /// 三人称視点
    /// </summary>
    private void SetThirdPersonCamera()
    {
        _ThirdPersonCamera.Priority = 10;
        _FirstPersonCamera.Priority = 0;
        _shutterButton.SetActive(false);
        _isFPS = false;
        _isTPS = true;
    }
    /// <summary>
    /// 一人称視点
    /// </summary>
    private void SetFirstPersonCamera() 
    {
        _ThirdPersonCamera.Priority = 0;
        _FirstPersonCamera.Priority = 10;
        _shutterButton.SetActive(true);
        _isFPS = true;
        _isTPS = false;
    }

    /// <summary>
    /// カメラの1人称視点中にシャッターを押すアクション
    /// </summary>
    public void Shutter()
    {
        if (_isFPS && !_flagTime && _restFilm > 0)
        {
            // FPS視点のみ適用
            audioSource.PlayOneShot(_shutterSound);
            _flashAction.ShutterEffect();
            _useFilm++;
            _forceTime = 0.2f;
            _flagTime = true;

            if (_isHit)
            {
                _isClear = true;
                Debug.Log("撮影成功！");
            }
            else
            {
                Debug.Log("撮影失敗");
            }
        }
    }

    /// <summary>
    /// 向いている方向へデバッグ用レーザーを発射し撮影対象に触れた場合、判定にチェックつける
    /// </summary>
    public void CheckTargetInCamera()
    {
        var rayStartPosition = _mainCamera.transform.position;
        var rayDirection = _mainCamera.transform.forward.normalized;
        RaycastHit raycastHit;
        var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);
        // Debug.DrawRay (Vector3 start(rayを開始する位置), Vector3 dir(rayの方向と長さ), Color color(ラインの色));
        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        if (isHit)
        {
            //レーザーが対象に触れているかどうかの判定
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                _isHit = true;
            }
            else
            {
                _isHit = false;
            }
        }

    }

    /// <summary>
    /// カメラの感度調整
    /// </summary>
    public void CameraSenseAdjust()
    {
        _input.look *= _lookSpeedAdjust;
    }
}
