using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance = null;

    [Header("�X�R�A")]public int score;
    [Header("���݂̃X�e�[�W")] public int stageNum;
    [Header("���݂̕��A�ʒu")] public int continureNum;
    [Header("���݂̎c�@")] public int heartNum;
    [Header("�f�t�H���g�̎c�@")] public int defaultHeartNum;
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
    /// �c�@���P���₷
    /// </summary>
    public void AddHeartNum()
    {
        if(heartNum < 99)
        {
            ++heartNum;
        }
    }


    /// <summary>
    /// �c�@���P���炷
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
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ�s����܂���");
        }
    }


}
