using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [Header("エフェクトが付いた床を判定するか")] public bool checkPlatformGround;

    private string groundTag = "Ground";
    private string platformTag = "GroundPlatform";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;
    // Start is called before the first frame update

    public bool IsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag)
        {
            isGroundEnter = true;
            //Debug.Log("地面が判定に入りました");
        }
        else if(checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundEnter = true;
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
            //Debug.Log("地面が判定内に入り続けています");
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundStay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == groundTag)
        {
            isGroundExit = true;
            //Debug.Log("地面が判定からでました");
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundExit = true;
        }

    }
}
