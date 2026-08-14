using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ActionManager : MonoBehaviour
{
    public UIController uIController;                                       // UIControllerスクリプト
    public TimeManager timeManager;                                         // TimeManagerスクリプト
    public SoundManager soundManager;                                       // SoundManagerスクリプト
    public EffectController effectController;                               // EffectControllerスクリプト
    public CameraManager cameraManager;                                     // CameraManagerスクリプト

    [SerializeField] private string _stageName;                             // STAGE名

    [SerializeField] private GameObject _player;                            // プレイヤーのゲームオブジェクト
    [SerializeField] private GameObject _itemObject;                        // item(シーン上のアイテム)
    [SerializeField] private GameObject _itemThrowObject;                   // item(投げるアイテム)

    public Sprite itemPickSprite;                                           // itempickのイメージ
    public Sprite itemThrowSprite;                                          // itemthrowのイメージ
    public Sprite screenShotSprite;                                         // ScreenShotのイメージ
    public Image actionImg;                                                 // Actionボタンのイメージ
    public Image flashPanel;                                                // 撮影エフェクトパネル

    public bool itemPickFlg = false;                                        // itempickフラグ
    public bool itemThrowFlg = false;                                       // itemthrowフラグ
    public bool filmFlg = false;

    public TextMeshProUGUI cameraShotText;                                  // 撮影回数テキスト
    public TextMeshProUGUI captureTargetText;                                      // 撮影対象テキスト
    public TextMeshProUGUI itemText;                                        // アイテムテキスト

    public int cameraShotCount = 10;                                        // 撮影回数カウント
    public int captureTargetCount = 1;                                             // 撮影対象カウント

    // Captureクラス(キャプチャデータ保存用)
    [System.Serializable]
    public class Capture
    {
        public string stageName { get; set; }
        public string capturePath { get; set; }
        public bool judge { get; set; }
    }

    // キャプチャリスト
    public List<Capture> cameraShotList = new List<Capture>();

    // キャプチャリスト（データ保存用）
    public static List<Capture> saveCaptureList = new List<Capture>();

    // Start is called before the first frame update
    void Start()
    {
        // 開始時のテキストを設定
        cameraShotText.text = "残り撮影回数:" + cameraShotCount;
        captureTargetText.text = "残り撮影対象:" + captureTargetCount;
        itemText.text = "アイテム：なし";

        // ActionUIButtonの初期イメージ
        actionImg.sprite = screenShotSprite;
    }

    // Update is called once per frame
    void Update()
    {
        // テキスト表示
        cameraShotText.text = "残り撮影回数:" + cameraShotCount;
        captureTargetText.text = "残り撮影対象:" + captureTargetCount;

        // アイテムを持っている場合
        if (itemThrowFlg)
        {
            itemText.text = "アイテム：あり";
        }
        else
        {
            itemText.text = "アイテム：なし";
        }

        // ゲームオーバー判定
        GameOver();
    }

    /// <summary>
    /// アクション機能
    /// <summary>
    public void Action()
    {
        // itemPickFlgがtrue、itemThrowFlgがfalse
        if (itemPickFlg && !itemThrowFlg)
        {
            // アイテム収集を実行
            ItemPick();
        }
        // itemPickFlgがfalse、itemThrowFlgがtrue
        else if (!itemPickFlg && itemThrowFlg)
        {
            // アイテム投下を実行
            ItemThrow();
        }
        // それ以外
        else
        {
            // 撮影機能を実行
            ScreenShot();
        }
    }

    /// <summary>
    /// 撮影機能
    /// <summary>
    public void ScreenShot()
    {
        // シャッター音を鳴らす
        soundManager.Play("シャッター音");

        // 撮影回数カウントを増やす
        cameraShotCount--;
        //撮影エフェクトを表示
        effectController.ShutterEffect();

        // カメラ撮影の判定
        if (cameraManager.rayCastFlg)
        {
            // 撮影成功
            filmFlg = true;
            // 撮影対象カウントを減らす
            captureTargetCount--;
        }
        else
        {
            // 撮影失敗
            filmFlg = false;
        }
        //Invorkeはキャプチャの保存とゲームクリア判定が正しく行われるための対策
        // スクリーンショットを保存する(2秒後) ※保存した写真を何かに試用してないためコメントアウト
        //Invoke(nameof(SavePicture), 2f);
        // ゲームクリア判定(2秒後)
        Invoke(nameof(GameClear), 3f);
    }

    /// <summary>
    /// アイテム収集
    /// </summary>
    public void ItemPick()
    {
        // アイテム収集時の効果音
        soundManager.Play("アイテム収集");

        // ボタンの画像差し替え
        actionImg.sprite = itemThrowSprite;

        // 収集アイテムを非表示
        _itemObject.gameObject.SetActive(false);

        // フラグの変更
        itemPickFlg = false;
        itemThrowFlg = true;
    }

    /// <summary>
    /// アイテム投下
    /// </summary>
    public async void ItemThrow()
    {
        // アイテム投下時の効果音
        soundManager.Play("アイテム投下");

        // ボタンの画像差し替え
        actionImg.sprite = screenShotSprite;

        // 投下アイテムの生成
        GameObject item = Instantiate(_itemThrowObject, new Vector3(_player.transform.position.x, 2, _player.transform.position.z), Quaternion.identity);

        // 投下時の設定
        item.GetComponent<Rigidbody>().AddForce(_player.transform.forward * 300);

        // フラグの変更
        itemPickFlg = false;
        itemThrowFlg = false;

        //1秒停止後、アイテム削除(撮影対象に当たらなかった場合の対処)
        await Task.Delay(1000);
        Destroy(item);
    }

    /// <summary>
    /// スクリーンショット保存
    /// <summary>
    private void SavePicture()
    {
        // ディレクトリ、キャプチャ等の名前を定義
        Capture c = new Capture();
        string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        var directory = Application.persistentDataPath + "/" + _stageName;
        var screenShotPicture = directory + "/" + _stageName + "_" + (10 - cameraShotCount) + "_" + date + ".png";

        // ステージのディレクトリがない場合
        if (!System.IO.Directory.Exists(directory))
        {
            // ステージのディレクトリ作成とスクリーンショットを保存
            System.IO.Directory.CreateDirectory(directory);
            ScreenCapture.CaptureScreenshot(screenShotPicture);

        }
        // ステージのディレクトリがある場合
        else
        {
            // スクリーンショットを保存
            ScreenCapture.CaptureScreenshot(screenShotPicture);
        }

        // 撮影結果のリストにスクリーンショットを入れる(成功、失敗ともに)
        c.stageName = _stageName;
        c.capturePath = screenShotPicture;
        if (filmFlg)
        {
            c.judge = true;
        }
        else
        {
            c.judge = false;
        }
        cameraShotList.Add(c);
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    public void GameClear()
    {
        // 撮影対象カウントが0になったらゲームクリア
        if (captureTargetCount == 0)
        {
            // 撮影したキャプチャリストを展開
            foreach (var cameraShot in cameraShotList)
            {
                // 成功したキャプチャか判定する
                if (cameraShot.judge)
                {
                    // 保存キャプチャリストに保存
                    saveCaptureList.Add(cameraShot);
                }
                else
                {
                    // 撮影失敗したキャプチャを削除する
                    System.IO.File.Delete(cameraShot.capturePath);
                }
            }
            // 撮影したキャプチャを確認したらゲームクリア
            uIController.ActiveGameClear();
        }
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameOver()
    {
        // 撮影回数が0かつ撮影フラグがfalse(撮影失敗)または制限時間が0になったらゲームオーバー
        if (cameraShotCount == 0 && !filmFlg || !timeManager.timerRunningFlg)
        {
            flashPanel.color = Color.clear;
            uIController.ActiveGameOver();
        }
    }
}
