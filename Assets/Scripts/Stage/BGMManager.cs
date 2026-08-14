using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    
    public Transform player;                       // プレイヤーの位置
    public Transform target;                       // ターゲットの位置
    public float detectionRange;                   // 検知範囲
    public SoundManager soundManager;              // SoundManager
    public EnemyController enemyController;        // EnemyController

    private bool _isBGM2Playing = false;           // BGM2フラグ

    // Start is called before the first frame update
    void Start()
    {
        // 初期BGMを再生する
        soundManager.Play("BGM1");
    }

    // Update is called once per frame
    void Update()
    {
        // ターゲット死亡フラグが立っている場合はBGM1
        if (enemyController.isTargetDeathFlg)
        {
            if (_isBGM2Playing)
            {
                SoundBGM();
            }
            return;
        }

        // プレイヤーと撮影対象の距離を取得
        float distance = Vector3.Distance(player.position, target.position);

        // detectionRangeは処理を起こす検知範囲
        if (distance < detectionRange && !_isBGM2Playing)
        {
            // 検知範囲内の場合、BGM2を流す
            SoundBGM2();
        }
        else if (distance >= detectionRange && _isBGM2Playing)
        {
            // 検知範囲外の場合、BGM1を流す
            SoundBGM();
        }
    }

    public void SoundBGM()
    {
        soundManager.Stop("BGM2");
        soundManager.Play("BGM1");
        _isBGM2Playing = false;
    }

    public void SoundBGM2()
    {
        soundManager.Stop("BGM1");
        soundManager.Play("BGM2");
        _isBGM2Playing = true;
    }
}
