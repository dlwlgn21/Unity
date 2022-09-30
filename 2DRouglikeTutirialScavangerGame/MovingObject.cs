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
        mBoxColider2D.enabled = false;                  // Ray�� ����� �� �ڽ��� colider�� �ε����� �ʱ� ����.
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
     * Generic type T�� �̵��� ������ ��, ������ ������ ������Ʈ Ÿ���� ����Ű�� ���� ����Ѵ�.
     * Enemy�� ����� ��쿡�� Player�� �� ���̰�,
     * Player�� ����� ��쿡�� InnerWall�� �� ����. �׷��� �÷��̾ �����ϰ� �ı��� �� ����
     */
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component     // where Ű����� T�� ������Ʈ ������ ����Ű�� �� ����.
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
            // ���⿡ �����ߴٴ� ��, ��ȣ�ۿ� �� �� �ִ� ������Ʈ�� �浹������ �ǹ���.
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
            yield return null;  // ������ �����ϱ� ���� ���� �������� ��ٸ�.
        }
    }

    protected abstract void OnCanNotMove<T>(T component)
        where T : Component;
}