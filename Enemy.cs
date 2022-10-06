using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth;
        public int Damage { get; set; }
        private int mCurrrentHealth;
        public int CurrentHealth 
        {
            get { return mCurrrentHealth; }
            set { mCurrrentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            CurrentHealth = maxHealth;
            Damage = 40;
        }
    }
    public EnemyStats stats = new EnemyStats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator mStatusIndicator;

    [SerializeField]
    private int enemyMoneyDropAmount;

    private bool mIsDestroyedByPlayer;
    void Start()
    {
        stats.Init();
        Debug.Assert(mStatusIndicator != null);
        mStatusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        GameMaster.gm.onToggleUpgrademenu += OnUpgradeMenuToggle;
        Debug.Assert(enemyMoneyDropAmount > 0);
        mIsDestroyedByPlayer = true;
    }

    public void DamageEnemy(int damage)
    {
        stats.CurrentHealth -= damage;
        if (stats.CurrentHealth <= 0)
        {
            if (mIsDestroyedByPlayer)
            {
                GameMaster.Money += enemyMoneyDropAmount;
            }
            GameMaster.gm.KillEnemy(this);
        }
        if (mStatusIndicator != null)
        {
            mStatusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.DamagePlayer(stats.Damage);
            mIsDestroyedByPlayer = false;
            DamageEnemy(9999);
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<EnemyAI>().enabled = !active;
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgrademenu -= OnUpgradeMenuToggle;
    }
}
