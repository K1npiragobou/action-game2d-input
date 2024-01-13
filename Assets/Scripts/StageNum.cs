using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNum : MonoBehaviour
{
    private Text stageText = null;
    private int oldStageNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        stageText = GetComponent<Text>();
        if (GManager.Instance != null)
        {
            stageText.text = "Stage" + GManager.Instance.stageNum;
        }
        else
        {
            Debug.Log("ゲームマネージャー置き忘れてるよ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldStageNum != GManager.Instance.stageNum)
        {
            stageText.text = "Stage" + GManager.Instance.stageNum;
            oldStageNum = GManager.Instance.stageNum;
        }
    }
}
