using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    private Text heartText = null;
    private int oldHeartNum;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<Text>();
        if (GManager.Instance != null)
        {
            heartText.text = "~" + GManager.Instance.heartNum;
        }
        else
        {
            Debug.Log("ƒQ[ƒ€ƒ}ƒl[ƒWƒƒ[’u‚«–Y‚ê‚Ä‚é‚æ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldHeartNum != GManager.Instance.heartNum)
        {
            heartText.text = "~" + GManager.Instance.heartNum;
            oldHeartNum = GManager.Instance.heartNum;
        }
    }
}
