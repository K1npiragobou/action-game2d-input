using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    //プレイヤーが判定内に入ったら
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
