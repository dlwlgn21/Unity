using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 2;
    private static int mRemainingLife;
    public static int RemainingLife { get { return mRemainingLife; } }

    [SerializeField]
    private int startingMoney;
    public static int Money;


    private void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }
    }
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float respawnDelay;
    public Transform respawnPrefab;
    public Transform enemyBoomPrefab;

    public CameraShaker camShaker;
    private float mCamShakeAmount = 0.3f;
    private float mCamShakeLengh = 0.3f;

    [SerializeField]
    private GameObject mGameOverUI;

    [SerializeField]
    private string explosionSound;

    [SerializeField]
    private string playerDeathSound;

    [SerializeField]
    private string playerRespawnSound;

    [SerializeField]
    private string playerSpawnSound;

    [SerializeField]
    private string gameOverSound;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgrademenu;

    private bool mIsRespawningPlayer = false;
    public bool IsGameover { get; set; }

    public IEnumerator RespawnPlayer()
    {
        mIsRespawningPlayer = true;
        AudioManager.instance.PlaySound(playerRespawnSound);
        yield return new WaitForSeconds(respawnDelay);
        PlayerStats.instance.CurrentHealth = PlayerStats.instance.MaxHealth;
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        mIsRespawningPlayer = false;

        GameObject particleInstance = Instantiate(respawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        AudioManager.instance.PlaySound(playerSpawnSound);
        Destroy(particleInstance, 3.0f);
    }

    void Start()
    {
        mRemainingLife = maxLives;
        Debug.Assert(explosionSound != null);
        Debug.Assert(playerDeathSound != null);
        Debug.Assert(upgradeMenu != null);
        Money = startingMoney;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            toggleUpgradeMenu();
        }
    }

    public void EndGame()
    {
        IsGameover = true;
        AudioManager.instance.PlaySound(gameOverSound);
        mGameOverUI.SetActive(true);
    }

    public void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        AudioManager.instance.PlaySound(playerDeathSound);
        --mRemainingLife;
        if (mRemainingLife <= 0)
        {
            gm.EndGame();
            return;
        }
        StartCoroutine(gm.RespawnPlayer());
        
    }

    public void KillEnemy(Enemy enemy)
    {
        Debug.Assert(camShaker != null);
        camShaker.Shake(mCamShakeAmount, mCamShakeLengh);
        GameObject particleInstance = Instantiate(enemyBoomPrefab, enemy.transform.position, enemy.transform.rotation).gameObject;
        Destroy(particleInstance, 3.0f);
        Destroy(enemy.gameObject);

        AudioManager.instance.PlaySound(explosionSound);
    }

    private void toggleUpgradeMenu()
    {
        if (IsGameover || mIsRespawningPlayer)
        {
            return;
        }
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgrademenu.Invoke(upgradeMenu.activeSelf);
    }
}
