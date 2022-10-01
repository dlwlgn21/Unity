using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2.0f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardManager;
    public int playerFoodPoint = 100;
    [HideInInspector] public bool isPlayerTurn = true;

    // UI
    private Text mLevelText;
    private GameObject mLevelImage;
    private bool mIsDoingSetup;                 // Setup �� �������� Player�� �������� ���ϰ� �� ����.

    private int mLevel = 1;
    private List<Enemy> mEnemies;
    private bool mIsEnemyMoving;
    void Awake()
    {
        Debug.Log("GameManager Awake");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        mEnemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();
        initGame();
    }

    private void hideLevelImage()
    {
        mLevelImage.SetActive(false);
        mIsDoingSetup = false;
    }

    void Update()
    {
        if (isPlayerTurn || mIsEnemyMoving || mIsDoingSetup)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    public void GameOver()
    {
        mLevelImage.SetActive(true);
        mLevelText.text = $"After {mLevel} days you starved.";
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
    void initGame()
    {
        mIsDoingSetup = true;
        mLevelImage = GameObject.Find("LevelImage");
        mLevelText = GameObject.Find("LevelText").GetComponent<Text>();
        mLevelText.text = $"Day {mLevel}";
        mLevelImage.SetActive(true);
        Invoke("hideLevelImage", levelStartDelay);
        mEnemies.Clear();
        boardManager.SetupScene(mLevel);
    }

    // UnityAPI��, ���� �ε�� ������ ȣ�� ��.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.mLevel++;
        instance.initGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

}
