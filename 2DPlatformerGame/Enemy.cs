using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth;

        private int mCurrrentHealth;
        public int CurrentHealth 
        {
            get { return mCurrrentHealth; }
            set { mCurrrentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            CurrentHealth = maxHealth;
        }
    }
    public EnemyStats enemyStats = new EnemyStats();
    
    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator mStatusIndicator;

    void Start()
    {
        enemyStats.Init();
        if (mStatusIndicator != null)
        {
            mStatusIndicator.SetHealth(enemyStats.CurrentHealth, enemyStats.maxHealth);
        }
    }

    public void DamageEnemy(int damage)
    {
        enemyStats.CurrentHealth -= damage;
        if (enemyStats.CurrentHealth <= 0)
        {
            GameMaster.gm.KillEnemy(this);
        }
        if (mStatusIndicator != null)
        {
            mStatusIndicator.SetHealth(enemyStats.CurrentHealth, enemyStats.maxHealth);
        }
    }
}
