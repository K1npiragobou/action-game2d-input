using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePoint : MonoBehaviour
{
    [Header("�R���e�j���[�ԍ�")] public int continueNum;
    [Header("��")] public AudioClip se;
    [Header("�v���C���[����")] public PlayerTriggerCheck trigger;
    [Header("�X�s�[�h")] public float speed = 2.0f;
    [Header("�擾�A�j���[�V����")] public AnimationCurve curve;

    private bool on;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        if(trigger == null || se == null)
        {
            Debug.Log("�C���X�y�N�^�[�̐ݒ肪����܂���");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[���͈͓��ɓ�����
        if(trigger.isOn && !on)
        {
            GManager.Instance.continureNum = continueNum;
            GManager.Instance.PlaySE(se);
            on = true;
        }

        if (on)
        {
            if(timer < 1.0f)
            {
                transform.localScale = Vector3.one * curve.Evaluate(timer);
                timer += speed * Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.one * curve.Evaluate(1.0f);
                gameObject.SetActive(false);
                on = false;
            }
        }
    }
}
