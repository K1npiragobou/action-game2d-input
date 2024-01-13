using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    [Header("�g��h���̃A�j���[�V����")] public AnimationCurve curve;
    [Header("�X�e�[�W�R���g���[���[")] public StageCtrl ctrl;

    private bool comp;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!comp)
        {
            if(timer < 1.0f)
            {
                transform.localScale = Vector3.one * curve.Evaluate(timer);
                timer += Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.one;
                ctrl.ChangeScene(GManager.Instance.stageNum + 1);
                comp = true;
            }
        }
    }
}
