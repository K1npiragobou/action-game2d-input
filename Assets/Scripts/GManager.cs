using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance = null;

    [Header("スコア")]public int score;
    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")] public int continureNum;
    [Header("現在の残機")] public int heartNum;
    [Header("デフォルトの残機")] public int defaultHeartNum;
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isStageClear;

    private AudioSource audioSource = null;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機を１つ増やす
    /// </summary>
    public void AddHeartNum()
    {
        if(heartNum < 99)
        {
            ++heartNum;
        }
    }


    /// <summary>
    /// 残機を１減らす
    /// </summary>
    public void SubHeartNum()
    {
        if(heartNum > 0)
        {
            -- heartNum;
        }
        else
        {
            isGameOver = true;
        }
    }
    
    public void RetryGame()
    {
        isGameOver = false;
        heartNum = defaultHeartNum;
        score = 0;
        stageNum = 1;
        continureNum = 0;

    }

    public void PlaySE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定sれ亭ません");
        }
    }


}
