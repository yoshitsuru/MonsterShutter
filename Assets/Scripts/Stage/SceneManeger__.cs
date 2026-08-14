using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManeger__ : MonoBehaviour
{
    private string _sceneName;                    // 現在アクティブなシーン

    void Start()
    {
        // 左向きを有効にする
        Screen.autorotateToLandscapeLeft = true;
        // 右向きを有効にする
        Screen.autorotateToLandscapeRight = true;
        // 画面の向きを自動回転に設定する
        Screen.orientation = ScreenOrientation.AutoRotation;

        // アクティブシーンを取得
        _sceneName = SceneManager.GetActiveScene().name;

    }
    /// <summary>
    /// ステージ番号に紐づいたステージを呼び出す
    /// </summary>
    /// <param name="stageNumber">ステージ番号</param>
    public void LoadStage(int stageNumber)
    {
        // 指定のステージ番号を呼び出す
        SceneManager.LoadScene(stageNumber);
    }
    /// <summary>
    /// RETRYボタンを押したとき、現在のシーンを再呼び出し
    /// </summary>
    public void OnClickRetryButton()
    {
        // 現在のシーンを呼び出す
        SceneManager.LoadScene(_sceneName);
    }
    /// <summary>
    /// タイトルに戻る
    /// </summary>
    public void OnClickEndButton()
    {
        // タイトルシーンを呼び出す
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// ステージ選択ボタンを押したとき、ステージ選択画面を呼び出す
    /// </summary>
    public void OnClickStageSelectButton()
    {
        // ステージ選択画面を呼び出す
        SceneManager.LoadScene(1);
    }
}
