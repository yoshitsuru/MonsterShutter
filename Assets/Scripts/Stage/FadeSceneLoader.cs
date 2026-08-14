using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeSceneLoader : MonoBehaviour
{
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    public string sceneTitle;
    // Start is called before the first frame update
    public void CallCoroutine()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }


    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;               // パネルを有効化
        float elapsedTime = 0.0f;               // 経過時間を初期化
        Color startColor = fadePanel.color;     // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                          // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);    // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t);  // パネルの色を変更してフェードアウト
            yield return null;                                      // 1フレーム待機
        }

        fadePanel.color = endColor;     // フェードが完了したら最終色に設定
        switch (sceneTitle)             // 呼び出すタイトルで分岐
        {
            case "MainScene":
                SceneManager.LoadScene(sceneTitle);
                break;
            case "StageSelectScene":
                SceneManager.LoadScene(sceneTitle);
                break;
            default:
                break;
        }
            

        
    }
}
