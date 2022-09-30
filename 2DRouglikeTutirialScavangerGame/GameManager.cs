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

    // Enemy들이 자신을 게임매니저에 등록하도록 하는 함수.
    public void AddEnemyToList(Enemy enemy)
    {
        mEnemies.Add(enemy);
    }

    // 연속적으로 한번에 하나씩 적을 옮기는데 사용될 코루틴 함수.
    IEnumerator MoveEnemies()
    {
        mIsEnemyMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (mEnemies.Count == 0)
        {
            // 대기하는 적이 없어도 일단 플레이어가 기다리게 할 것임.
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < mEnemies.Count; ++i)
        {
            mEnemies[i].MoveEnemy();
            // 다음 enemy에 접근하기 전에, yield 키워드와 MovingObject의 moveTime 변수를 인자로 넘겨줘서 그 시간만큼 기다리게 할 것임.
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
