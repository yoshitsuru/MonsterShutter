using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timelimit;                     // 制限時間
    public float remainingTime;                 // 残り時間

    public bool timerRunningFlg = false;        // タイマー始動中フラグ
    private TextMeshProUGUI _textTimelimit;     // 制限時間のテキストオブジェクト

    void Start()
    {
        // 制限時間を表すテキストオブジェクトを取得
        _textTimelimit = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        // タイマーを開始
        timerRunningFlg = true;
        // 残り時間を制限時間に設定
        remainingTime = timelimit;
    }

    void Update()
    {
        // タイマー始動中の場合
        if (timerRunningFlg)
        {
            // 残り時間が0より大きい場合
            if (remainingTime > 0)
            {
                // 時間経過で残り時間が減少
                remainingTime -= Time.deltaTime;
                // 残り時間を表示
                DisplayTime(remainingTime);
            }
            else
            {
                // 残り時間を0に設定
                remainingTime = 0;
                // タイマーを停止
                timerRunningFlg = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // 表示時間の調整
        timeToDisplay += 1;
        // 分数の計算
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        // 秒数の計算
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        // 残り時間を文字列に変換し取得
        _textTimelimit.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
