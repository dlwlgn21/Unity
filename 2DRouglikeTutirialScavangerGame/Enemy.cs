using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamagePoint;


    private Animator mAnimator;
    private Transform mTarget;              // Enemy�� ���� ������ �˷��ִ� ����
    private bool mIsSkipMove;               // Enemy�� �ϸ��� �����̰� �ϴµ� ����� ����.
   
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        mAnimator = GetComponent<Animator>();
        mTarget = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    // Enemy�� �����̷� �� ������ ���� �Ŵ����� ���� ȣ�� �� �Լ�
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

    // Enemy�� Player���� �̵��Ϸ��� ���� �� ȣ�� ��.
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
