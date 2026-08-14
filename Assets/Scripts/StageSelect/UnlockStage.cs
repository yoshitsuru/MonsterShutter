using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnlockStage : MonoBehaviour
{
    
    [SerializeField]
    private Button[] _stageButton;              // ボタンオブジェクト
  
    void Start()
    {
        // アンロックしたステージ番号を保存
        //int stageUnlockCount = PlayerPrefs.GetInt("StageUnlock", 1);
        for (int i = 0; i < _stageButton.Length; i++)
        {
            // ステージ選択ボタンがアンロックした値より小さい場合
            //if (i < stageUnlockCount)
            //{
                // アンロックしたステージ選択ボタンをアクティブに
                _stageButton[i].interactable = true;
            //}
            //else
            ///{
                // アンロックしていないステージ選択ボタンを非アクティブのまま
            //    _stageButton[i].interactable = false;
            //}
        }
    }
    /// <summary>
    /// 選択したステージ番号のシーンを呼び出す
    /// </summary>
    /// <param name="stage">ステージ番号</param>
    public void StageSelect(int stage)
    {
        // 選択したステージ番号のシーンを呼び出す
        SceneManager.LoadScene(stage);
    }
}