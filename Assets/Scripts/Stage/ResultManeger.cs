using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ResultManeger : MonoBehaviour
{

    [Header("Game Result Maneger")]
    [SerializeField] private TimeManeger _timeManeger;
    [SerializeField] private CameraManeger _cameraManeger;
    [SerializeField] private ThirdPersonController _Player;

    public GameObject _resultPanel;         // リザルト画面

    public TextMeshProUGUI _textGameClear;  // ゲームクリアのテキスト
    public TextMeshProUGUI _textGameOver;  // ゲームオーバーのテキスト

    // Start is called before the first frame update
    void Start()
    {
        _textGameClear.text = "";
        _textGameOver.text = "";
        _resultPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 撮影対象を撮影できたらクリア
        if (_cameraManeger._isClear)
        {
            GameClear();
            return;
        }

        // 残りフィルムが0もしくは時間切れでゲームオーバー
        // ゲームのクリア判定が先なのでクリア状態でないでフィルムが0ならゲームオーバー
        if (_cameraManeger._restFilm == 0 || _timeManeger._isTimeUp || _Player._isEnemyAtack)
        {
            GameOver();
            return;
        }

    }

    public void GameClear()
    {
        Debug.Log("ゲームクリア");
        _resultPanel.SetActive(true);
        _textGameClear.text = "Game Clear";
        _textGameClear.color = Color.green;
    }

    public void GameOver()
    {
        Debug.Log("ゲームオーバー");
        _resultPanel.SetActive(true);
        _textGameOver.text = "Game Over";
        _textGameOver.color = Color.red;
    }

    public void PausePanel()
    {
        _resultPanel.SetActive(true);
    }

    public void BackButton()
    {
        _resultPanel.SetActive(false);
    }
}
