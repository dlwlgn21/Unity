using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int wallDamagePoint = 1;
    public int pointPerFood = 10;
    public int pointPerSoda = 20;
    public float restartLevelDelay = 1.0f;
    public Text foodText;

    // Audio Section
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameoverSound1;


    private Animator mAnimator;
    private int mFoodPoint;       // ������ �ٲٸ鼭 ���ھ �ٽ� ���ӸŴ����� �����ϱ� ����, �ش� ���������� �÷��̾� ���ھ ������ ����

    // ���� �÷��̾ ������ �� ȣ�� ��.
    public void LoseFoodPoint(int loss)
    {
        mAnimator.SetTrigger("PlayerHit");
        mFoodPoint -= loss;
        foodText.text = $"-{loss} Food : {mFoodPoint}";
        checkIfGameOver();
    }

    protected override void Start()
    {
        mAnimator = GetComponent<Animator>();
        mFoodPoint = GameManager.instance.playerFoodPoint;
        setFoodScore(mFoodPoint);
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
                foodText.text = $"+{pointPerFood} Food : {mFoodPoint}";
                SoundManager.instance.PlayRandomSfx(eatSound1, eatSound2);
                collision.gameObject.SetActive(false);
                break;
            case "Soda":
                mFoodPoint += pointPerSoda;
                foodText.text = $"+{pointPerSoda} Food : {mFoodPoint}";
                SoundManager.instance.PlayRandomSfx(drinkSound1, drinkSound2);
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
        setFoodScore(mFoodPoint);
        base.AttemptMove<T>(xDir, yDir);

        SoundManager.instance.PlayRandomSfx(moveSound1, moveSound2);
      
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
        // SceneManager.LoadScene(0);
        // Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }

    private void checkIfGameOver()
    {
        if (mFoodPoint <= 0)
        {
            SoundManager.instance.PlaySingle(gameoverSound1);
            SoundManager.instance.musicSrc.Stop();
            GameManager.instance.GameOver();
        }
    }
    void Update()
    {
        if (!GameManager.instance.isPlayerTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

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
    private void setFoodScore(int score)
    {
        foodText.text = $"Food : {mFoodPoint}";
    }
}
