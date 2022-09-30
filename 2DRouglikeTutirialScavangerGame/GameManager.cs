using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardManager;
    public int playerFoodPoint = 100;
    [HideInInspector] public bool isPlayerTurn = true;

    
    private int level = 3;
    private List<Enemy> mEnemies;
    private bool mIsEnemyMoving;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Debug.Log("Call Awake()");
        DontDestroyOnLoad(gameObject);
        mEnemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();
        InitGame();
    }
    void Update()
    {
        if (isPlayerTurn || mIsEnemyMoving)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    public void GameOver()
    {
        enabled = false;
    }

    // Enemy���� �ڽ��� ���ӸŴ����� ����ϵ��� �ϴ� �Լ�.
    public void AddEnemyToList(Enemy enemy)
    {
        mEnemies.Add(enemy);
    }

    // ���������� �ѹ��� �ϳ��� ���� �ű�µ� ���� �ڷ�ƾ �Լ�.
    IEnumerator MoveEnemies()
    {
        mIsEnemyMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (mEnemies.Count == 0)
        {
            // ����ϴ� ���� ��� �ϴ� �÷��̾ ��ٸ��� �� ����.
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < mEnemies.Count; ++i)
        {
            mEnemies[i].MoveEnemy();
            // ���� enemy�� �����ϱ� ����, yield Ű����� MovingObject�� moveTime ������ ���ڷ� �Ѱ��༭ �� �ð���ŭ ��ٸ��� �� ����.
            yield return new WaitForSeconds(mEnemies[i].moveTime);
        }
        isPlayerTurn = true;
        mIsEnemyMoving = false;
    }
    public void InitGame()
    {
        mEnemies.Clear();
        Debug.Log("Call InitGame()");
        boardManager.SetupScene(level);
    }
}
