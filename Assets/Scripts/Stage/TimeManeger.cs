using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class TimeManeger : MonoBehaviour
{
    public float Timelimit = 180.0f;        // 制限時間
    public TextMeshProUGUI _textTimelimit;  // 制限時間のテキストオブジェクト

    public bool _isTimeUp;                  // タイムアップのフラグ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 制限時間のタイマー&制限時間判定
        if (Timelimit > 0)
        {
            _isTimeUp = false;
            Timelimit -= Time.deltaTime;
        }
        else if (Timelimit <= 0)
        {
            _isTimeUp = true;
            Timelimit = 0;
        }

        // テキストの制御
        _textTimelimit.text = $"制限時間：{Timelimit.ToString("#.#")}秒";
        if (Timelimit < 20.0f)
        {
            _textTimelimit.color = Color.red;
        }

    }
}
