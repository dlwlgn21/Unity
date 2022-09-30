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
    private int mFoodPoint;       // ������ �ٲٸ鼭 ���ھ �ٽ� ���ӸŴ����� �����ϱ� ����, �ش� ���������� �÷��̾� ���ھ ������ ����

    // ���� �÷��̾ ������ �� ȣ�� ��.
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

    // Generic T�� �����̴� ������Ʈ�� ����ĥ ����� ������Ʈ�� Ÿ���� �� ����.
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

    // �÷��̾ �ⱸ ������Ʈ�� �浹���� �� ȣ��. ��, ���������� �Ѿ
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
            // �÷��̾ �밢������ �������� ���ϰ� �ϱ� ����
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<InnerWall>(horizontal, vertical);
        }
    }
}
