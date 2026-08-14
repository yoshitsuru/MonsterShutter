using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    // クラスの値をインスペクター上で変更するため
    [System.Serializable]
    public class SoundData
    {
        public string name;             // 音声データのキー
        public AudioClip audioClip;     // 音声データ
        public float playedTime;        // 前回再生した時間
        public bool loopFlg;            // 音声データをループするか判定するフラグ
    }

    [SerializeField]
    private SoundData[] _soundDatas;                                // サウンドデータの配列 
    private AudioSource[] _audioSourceList = new AudioSource[20];   // AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意
    private Dictionary<string, SoundData> _soundDictionary
        = new Dictionary<string, SoundData>();                      // 別名(name)をキーとした管理用Dictionary
    [SerializeField]
    private float _playableDistance = 0.2f;                         // 一度再生してから、次再生出来るまでの間隔(秒)

    private void Awake()
    {
        // auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        for (var i = 0; i < _audioSourceList.Length; ++i)
        {
            _audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }

        //_soundDictionaryにセット
        foreach (var soundData in _soundDatas)
        {
            _soundDictionary.Add(soundData.name, soundData);
        }
    }

    /// <summary>
    /// 未使用のAudioSourceの取得
    /// 全て使用中の場合はnullを返却
    /// </summary>
    /// <returns></returns>
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < _audioSourceList.Length; ++i)
        {
            if (_audioSourceList[i].isPlaying == false)
            {
                return _audioSourceList[i];
            }
        }
        //未使用のAudioSourceは見つかりませんでした
        return null;
    }

    /// <summary>
    /// 指定されたAudioClipを未使用のAudioSourceで再生
    /// loopFlg 初期値:Flase
    /// </summary>
    /// <param name="clip"></param>
    public void Play(AudioClip clip, bool loopFlg = false)
    {
        // 未使用のAudioSourceの取得
        var audioSource = GetUnusedAudioSource();
        // audioClipがないもしくはすべて使用中の場合
        if (audioSource == null)
        {
            return;
        }
        audioSource.clip = clip;
        // 指定のaudioSourceを再生
        audioSource.Play();
    }

    /// <summary>
    /// 指定された別名で登録されたAudioClipを再生
    /// </summary>
    /// <param name="name">音声データのキー</param>
    // 引数として渡すものによって処理が分岐されるほうが使う側からすると理解しやすいのでこのオーバーロードを使いました
    public void Play(string name)
    {
        //管理用Dictionary から、別名で探索し、一致した場合
        if (_soundDictionary.TryGetValue(name, out var soundData))
        {
            // 再生可能時間より早い場合
            if (Time.realtimeSinceStartup - soundData.playedTime < _playableDistance)
            {
                return;
            }
            //次回再生用に今回の再生時間の保持
            soundData.playedTime = Time.realtimeSinceStartup;
            // 一致したaudioClipを再生
            Play(soundData.audioClip, soundData.loopFlg);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
    /// <summary>
    /// 指定された別名で登録されたAudioClipを止める
    /// </summary>
    /// <param name="clip"></param>
    public void Stop(AudioClip clip)
    {
        // AudioSourceの取得
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        // 指定のaudioSourceを止める
        audioSource.Stop();
    }
    /// <summary>
    /// 指定された別名で登録されたAudioClipを止める
    /// </summary>
    /// <param name="name">音声データのキー</param>
    public void Stop(string name)
    {
        // 管理用Dictionary から、別名で探索し、一致した場合
        if (_soundDictionary.TryGetValue(name, out var soundData))
        {
            // 一致したaudioClipを止める
            Stop(soundData.audioClip);
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
}
