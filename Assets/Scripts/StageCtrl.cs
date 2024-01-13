using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("�v���C���[�Q�[���I�u�W�F�N�g")] public GameObject playerObj;
    [Header("�R���e�j���[�ʒu")] public GameObject[] continuePoint;
    [Header("�Q�[���I�[�o�[")] public GameObject gameOverObj;
    [Header("�t�F�[�h")] public FadeImage fade;
    [Header("�Q�[���I�[�o�[SE")] public AudioClip gameOverSE;
    [Header("���g���CSE")] public AudioClip retrySE;
    [Header("�X�e�[�W�N���ASE")] public AudioClip stageClearSE;
    [Header("�X�e�[�W�N���A")] public GameObject stageClearObj;
    [Header("�X�e�[�W�N���A����")] public PlayerTriggerCheck stageClearTrigger;


    private Player p;
    private int nextStageNum;
    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doClear = false;

    // Start is called before the first frame update
    void Start()
    {
        if(playerObj != null && continuePoint != null && continuePoint.Length > 0 && gameOverObj != null && fade != null )
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);
            playerObj.transform.position = continuePoint[0].transform.position;

            p = playerObj.GetComponent<Player>();
            if( p == null )
            {
                Debug.Log("�v���C���[����Ȃ����̂��A�^�b�`����Ă����");
            }
        }
        else
        {
            Debug.Log("�ݒ肪����ĂȂ���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���I�[�o�[���̏���
        if(GManager.Instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            doGameOver = true;
        }
        //�v���C���[�����ꂽ���̏���
        else if(p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if(continuePoint.Length > GManager.Instance.continureNum)
            {
                playerObj.transform.position = continuePoint[GManager.Instance.continureNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("�R���e�j���[�|�C���g�̐ݒ肪����ĂȂ���");
            }
        }
        else if(stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;

        }

        //�X�e�[�W��؂�ւ���
        if(fade != null && startFade && !doSceneChange)
        {
            if(fade.IsFadeOutComplete())
            {
                //�Q�[�����g���C
                if (retryGame)
                {
                    GManager.Instance.RetryGame();
                }
                //���̃X�e�[�W
                else
                {
                    GManager.Instance.stageNum = nextStageNum;
                }
                GManager.Instance.isStageClear = false;
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;

            }
        }
    }


    /// <summary>
    /// �ŏ�����n�߂�
    /// </summary>
    public void Retry()
    {
        ChangeScene(1);//�ŏ��̃X�e�[�W�ɖ߂�̂łP
        retryGame = true;
    }
    /// <summary>
    /// �X�e�[�W��؂�ւ��܂�
    /// </summary>
    /// <param name="num"></param>
    public void ChangeScene(int num)
    {
        if(fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }

    /// <summary>
    /// �X�e�[�W���N���A����
    /// </summary>
    public void StageClear()
    {
        GManager.Instance.isStageClear = true;
        stageClearObj.SetActive(true);
        GManager.Instance.PlaySE(stageClearSE);
    }
}
