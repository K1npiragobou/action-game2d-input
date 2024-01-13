using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//インスペクターでいじれるように
    [Header("移動速度")] public float speed; //移動速度
    [Header("ジャンプ速度")] public float jumpSpeed; //ジャンプ速度
    [Header("ジャンプする高さ")] public float jumpHeight; //ジャンプする高さ
    [Header("ジャンプ制限時間")] public float jumpLimitTime; //ジャンプ制限時間
    [Header("重力")] public float gravity;//重力
    [Header("踏みつけた判定になる高さ")]public float stepOnRate;
    [Header("接地判定")] public GroundCheck ground;//接地判定
    [Header("天井判定")] public GroundCheck head; //頭をぶつけた判定
    [Header("ダッシュ加速度")] public AnimationCurve dashCurve;
    [Header("ジャンプ加速度")] public AnimationCurve jumpCurve;
    [Header("ジャンプするときにならすSE")] public AudioClip jumpSE;
    [Header("ダメージを食らったときにならすSE")] public AudioClip loseSE;//?
    [Header("ゲームオーバーするときにならすSE")] public AudioClip dieSE;//no
    [Header("リトライするときにならすSE")] public AudioClip retrySE;//no
    [Header("敵を踏んだときにならすSE")] public AudioClip otherjumpSE;//?
    #endregion
    #region//プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capcol = null;
    private SpriteRenderer sr = null;
    private MoveObject moveObj = null;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isLose = false;//youtubeではisDown
    private bool isOtherJump = false;
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private float jumpPos = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float jumpTime = 0.0f; //滞空時間を測る
    private float dashTime = 0.0f;
    private float beforeKey = 0.0f;
    private string enemyTag = "Enemy";
    private string deadAreaTag = "DeadArea";
    private string hitAreaTag = "HitArea";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isContinue){
            //明滅　ついているときの戻る
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えているとき
            else if(blinkTime > 0.1f){
                sr.enabled = false;
            }
            //明滅　ついているとき
            else
            {
                sr.enabled = true;
            }
        }

        //１秒経ったら明滅終わり
        if(continueTime > 2.0f)
        {
            isContinue = false;
            blinkTime = 0.0f;
            continueTime = 0.0f;
            sr.enabled = true;
        }
        else
        {
            blinkTime += Time.deltaTime;
            continueTime += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isLose && !GManager.Instance.isGameOver && !GManager.Instance.isStageClear)
        {
            //設置判定を受け取る
            isGround = ground.IsGround();
            isHead = head.IsGround();
            //キーが入力されたら動く

            float ySpeed = GetYSpeed();
            float xSpeed = GetXSpeed();
            //アニメーションを使用
            SetAnimation();

            //移動速度を設定
            Vector2 addVelocity = Vector2.zero;
            if(moveObj != null)
            {
                addVelocity = moveObj.GetVelocity();
            }
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;
        }
        else
        {
            if(!isClearMotion && GManager.Instance.isStageClear)
            {
                anim.Play("player_clear");
                isClearMotion = true;
            }
            rb.velocity = new Vector2(0, -gravity);
        }
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。
    /// </summary>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        //何かを踏んだ際のジャンプ
        if (isOtherJump)
        {
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;

            if (canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        //地面にいるとき
        else if (isGround)
        {
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    GManager.Instance.PlaySE(jumpSE);//SE
                }
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        //ジャンプ中
        else if (isJump)
        {
            //上方向キーを押しているか
            bool pushUpKey = verticalKey > 0;
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;

            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }
        return ySpeed;
    }
    /// <summary>
    /// X成分で必要な計算をして、速度を返す。
    /// </summary>
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;
        if (horizontalKey > 0)
        {
            isRun = true;
            transform.localScale = new Vector3(1, 1, 1);
            dashTime += Time.deltaTime;
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            isRun = true;
            transform.localScale = new Vector3(-1, 1, 1);
            dashTime += Time.deltaTime;
            xSpeed = -speed;
        }
        else
        {
            isRun = false;
            dashTime = 0.0f;
            xSpeed = 0.0f;
        }
        //前回の入力からダッシュの反転を判断して速度を変える。
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;
        //アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);
        return xSpeed;
    }
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }
    #region
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);

        if (enemy || moveFloor || fallFloor)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {
                if (p.point.y < judgePos)
                {
                    if(enemy || fallFloor)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o != null)
                        {
                            if (enemy)
                            {
                                otherJumpHeight = o.boundHeight;    //踏んづけたものから跳ねる高さを取得する
                                o.playerStepOn = true;        //踏んづけたものに対して踏んづけた事を通知する
                                jumpPos = transform.position.y; //ジャンプした位置を記録する
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if (fallFloor)
                            {
                                o.playerStepOn = true;
                            }
                            
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionが付いてないよ!");
                        }
                    }
                    else if (moveFloor)
                    {
                        moveObj = collision.gameObject.GetComponent<MoveObject>();
                    }
                    
                }
                else
                {
                    if (enemy)
                    {
                        ReceiveDamage(true);
                        break;
                    }
                }
            }
        }
        else if (collision.collider.tag == moveFloorTag)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {
                //動く床に乗っている
                if (p.point.y < judgePos)
                {
                    moveObj = collision.gameObject.GetComponent<MoveObject>();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            //動く床から離れた
            moveObj = null;
        }
    }


    #endregion//接触判定

    /// <summary>
    /// コンテニュー待機状態か
    /// </summary>
    /// <returns></returns>


    public bool IsContinueWaiting()
    {
        if (GManager.Instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }
 
    }

    //ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
        if(isLose && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("player_lose"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// コンテニューする
    /// </summary>
    public void ContinuePlayer()
    {
        isLose = false;
        anim.Play("player_stand");
        isJump = false;
        isOtherJump= false;
        isRun = false;
        isContinue = true;
        nonDownAnim = false;
    }

    //やられた時の処理
    private void ReceiveDamage(bool downAnim)
    {
        if (isLose || GManager.Instance.isStageClear)//クリア中はダメージを受けないように
        {
            return;
        }else
        {
            if (downAnim)
            {
                if (!isLose)
                {
                    GManager.Instance.PlaySE(loseSE);//うまくいくか微妙
                }
                anim.Play("player_lose");
            }
            else
            {
                {
                    nonDownAnim = true;
                }
            }

            isLose = true;
            GManager.Instance.SubHeartNum();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);
        }
        else if(collision.tag == hitAreaTag)
        {
            ReceiveDamage(true);
        }
    }

}
