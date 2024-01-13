using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private  Text scoreText = null;
    private int oldScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        if(GManager.Instance != null)
        {
            scoreText.text = "Score" + GManager.Instance.score;
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
        if(oldScore != GManager.Instance.score)
        {
            scoreText.text = "Score" + GManager.Instance.score;
            oldScore = GManager.Instance.score;
        }
    }
}
