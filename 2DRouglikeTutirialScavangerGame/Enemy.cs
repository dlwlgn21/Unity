using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamagePoint;


    private Animator mAnimator;
    private Transform mTarget;              // Enemy가 어디로 향할지 알려주는 변수
    private bool mIsSkipMove;               // Enemy가 턴마다 움직이게 하는데 사용할 것임.
   
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        mAnimator = GetComponent<Animator>();
        mTarget = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    // Enemy를 움직이려 할 때마다 게임 매니저에 의해 호출 될 함수
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(mTarget.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = mTarget.position.y > transform.position.y ? 1 : -1;
            
        }
        else
        {
            xDir = mTarget.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove<Player>(xDir, yDir);
    }
    

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (mIsSkipMove)
        {
            mIsSkipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        mIsSkipMove = true;
    }

    // Enemy가 Player에게 이동하려고 했을 때 호출 됨.
    protected override void OnCanNotMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        Debug.Assert(hitPlayer != null);
        mAnimator.SetTrigger("EnemyChop");
        hitPlayer.LoseFoodPoint(playerDamagePoint);
    }

    void Update()
    {
        
    }
}
