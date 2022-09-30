using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 25.0f;
    public LayerMask blockingLayer;

    private BoxCollider2D mBoxColider2D;
    private Rigidbody2D mRb2D;
    private float mInverseMoveTime;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        mBoxColider2D = GetComponent<BoxCollider2D>();
        mRb2D = GetComponent<Rigidbody2D>();
        mInverseMoveTime = 1f / moveTime;
    }

    protected bool TryMove(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);  
        mBoxColider2D.enabled = false;                  // Ray를 사용할 떄 자신의 colider에 부딪히지 않기 위함.
        hit = Physics2D.Linecast(start, end, blockingLayer);
        mBoxColider2D.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    /* 
     * Generic type T는 이동이 막혔을 때, 유닛이 반응할 컴포넌트 타입을 가리키기 위해 사용한다.
     * Enemy에 적용된 경우에는 Player가 될 것이고,
     * Player에 적용된 경우에는 InnerWall이 될 것임. 그래서 플레이어가 공격하고 파괴할 수 있음
     */
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component     // where 키워드로 T가 컴포넌트 종류를 가리키게 할 것임.
    {
        RaycastHit2D hit;
        bool isCanMove = TryMove(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!isCanMove && hitComponent != null)
        {
            // 여기에 진입했다는 건, 상호작용 할 수 있는 오브젝트와 충돌했음을 의미함.
            OnCanNotMove<T>(hitComponent);
        }

    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(mRb2D.position, end, 25.0f * Time.deltaTime);
            mRb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;  // 루프를 갱신하기 전에 다음 프레임을 기다림.
        }
    }

    protected abstract void OnCanNotMove<T>(T component)
        where T : Component;
}