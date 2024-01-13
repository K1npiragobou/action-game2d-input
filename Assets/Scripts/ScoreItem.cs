using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("���Z����X�R�A")] public int myScore;
    [Header("�v���C���[�̔���")] public PlayerTriggerCheck playerCheck;
    //�v���C���[��������ɓ�������
    void Update()
    {
        if (playerCheck.isOn)
        {
            if(GManager.Instance != null)
            {
                GManager.Instance.score += myScore;
                Destroy(this.gameObject);
            }
        }
    }
}
