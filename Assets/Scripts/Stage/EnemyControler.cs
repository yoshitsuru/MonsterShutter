using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class EnemyControler : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;

    //private float _speed = 2.0f;

    private float _animationBlend;
    // animation IDs
    private int _animIDSpeed;
    private int _animIDMotionSpeed;

    private Animator _animator;
    private bool _hasAnimator;

    public Transform[] goals;
    private int destNum = 0;

    //[Tooltip("AudioClip")]
    //public AudioClip LandingAudioClip;
    //public AudioClip[] FootstepAudioClips;
    //[Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
        //_hasAnimator = TryGetComponent(out _animator);
        //AssignAnimationIDs();
    }

    // Update is called once per frame
    void Update()
    {
        //_hasAnimator = TryGetComponent(out _animator);

        EnemyMove();
    }

    void nextGoal()
    {

        destNum += 1;
        if (destNum == 4)
        {
            destNum = 0;
        }

        // ランダムに巡回する場合、destNum = Random.Range(0,4);
        agent.destination = goals[destNum].position;

        Debug.Log(destNum);
    }

    private void EnemyMove()
    {
        // NaviMeshで範囲内に入ったプレイヤーを追尾する処理
        if (target.GetComponent<ThirdPersonController>()._isArea == true)
        {
            agent.speed = 3.5f;
            agent.destination = target.transform.position;
        }
        // 
        else
        {
            // Debug.Log(agent.remainingDistance);
            if (agent.remainingDistance < 0.5f)
            {
                agent.speed = 2.0f;
                nextGoal();
            }
        }

    }

    ///// <summary>
    ///// アニメーションIDを呼び出す
    ///// </summary>
    //private void AssignAnimationIDs()
    //{
    //    _animIDSpeed = Animator.StringToHash("Speed");
    //    _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    //}
}
