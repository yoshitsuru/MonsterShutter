using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject pauseCanvas;      // ポーズ画面
    public GameObject gameOverCanvas;   // ゲームオーバー画面
    public GameObject gameClearCanvas;  // ゲームクリア画面
    public GameObject effectPanel;      // エフェクトパネル(撮影時のフラッシュ)

    public bool pauseFlg;               //ポーズフラグ

    void Start()
    {
        // 時間の速さを初期値1.0fで設定
        Time.timeScale = 1.0f;
        // ポーズフラグの初期値をFalseで設定
        pauseFlg = false;
    }

    /// <summary>
    /// ポーズボタンを押したときの処理
    /// </summary>
    public void OnClickPauseButton()
    {
        // ポーズフラグがFalseの場合
        if (!pauseFlg)
        {
            // ポーズ画面を表示
            pauseCanvas.SetActive(true);
            // ゲーム時間を停止
            Time.timeScale = 0.0f;
            // ポーズフラグをTrueに
            pauseFlg = true;
        }
        else
        {
            // ポーズ画面を非表示
            pauseCanvas.SetActive(false);
            // ゲーム時間を通常に設定
            Time.timeScale = 1.0f;
            // ポーズフラグをFalseに
            pauseFlg = false;
        }
    }

    public void ActiveGameOver()
    {
        // ゲームオーバー画面を表示
        gameOverCanvas.SetActive(true);
        // ゲーム時間を停止
        Time.timeScale = 0.0f;
    }
    public void ActiveGameClear()
    {
        // ゲームクリア画面を表示
        gameClearCanvas.SetActive(true);
        // ゲーム時間を停止
        Time.timeScale = 0.0f;
    }
}
