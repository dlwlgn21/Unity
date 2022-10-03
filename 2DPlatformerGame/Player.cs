using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int health;
    }
    public int initialHealth;
    public PlayerStats playerStats = new PlayerStats();

    private float deathLimitHeight = -20;

    void Update()
    {
        if (transform.position.y <= deathLimitHeight)
        {
            DamagePlayer(1000);
        }
    }
    public void DamagePlayer(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            GameMaster.gm.KillPlayer(this);
            Debug.Log("GameMaster.KillPlayer(this)");
        }
    }

}
