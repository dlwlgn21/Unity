using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamagePoint = 1;
    public int pointPerFood = 10;
    public int pointPerSoda = 20;
    public float restartLevelDelay = 1.0f;

    private Animator mAnimator;
    private int mFoodPoint;       // 레벨을 바꾸면서 스코어를 다시 게임매니저로 전달하기 전에, 해당 레벨동안의 플레이어 스코어를 저장할 변수

    // 적이 플레이어를 공격할 때 호출 됨.
    public void LoseFoodPoint(int loss)
    {
        mAnimator.SetTrigger("PlayerHit");
        mFoodPoint -= loss;
        checkIfGameOver();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        mAnimator = GetComponent<Animator>();
        mFoodPoint = GameManager.instance.playerFoodPoint;

        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Exit":
                Debug.Log("Enter Exit Statement");
                Invoke("restart", restartLevelDelay);
                enabled = false;
                break;
            case "Food":
                mFoodPoint += pointPerFood;
                collision.gameObject.SetActive(false);
                break;
            case "Soda":
                mFoodPoint += pointPerSoda;
                collision.gameObject.SetActive(false);
                break;
            default:
                Debug.Assert(false);
                break;
        }

    }

    // Generic T는 움직이는 오브젝트가 마주칠 대상의 컴포넌트의 타입이 될 것임.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        --mFoodPoint;
        base.AttemptMove<T>(xDir, yDir);

        checkIfGameOver();
        GameManager.instance.isPlayerTurn = false;

    }

    protected override void OnCanNotMove<T>(T component)
    {
        InnerWall damagedWall = component as InnerWall;
        damagedWall.DamageWall(wallDamagePoint);
        mAnimator.SetTrigger("PlayerChop");

    }
    
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoint = mFoodPoint;
    }

    // 플레이어가 출구 오브젝트와 충돌했을 때 호출. 즉, 다음레벨로 넘어감
    private void restart()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Enter restart");
        Debug.Assert(GameManager.instance != null);
        GameManager.instance.InitGame();
        Debug.Log("After InitGame()");

        // Application.LoadLevel(Application.loadedLevel);
    }

    private void checkIfGameOver()
    {
        if (mFoodPoint <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPlayerTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            // 플레이어가 대각선으로 움직이지 못하게 하기 위함
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<InnerWall>(horizontal, vertical);
        }
    }
}
