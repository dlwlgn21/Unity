using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

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
    public AudioSource audio;
    public IEnumerator RespawnPlayer()
    {
        audio.Play();
        yield return new WaitForSeconds(respawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject particleInstance = Instantiate(respawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(particleInstance, 3.0f);
    }

    public void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        StartCoroutine(gm.RespawnPlayer());
    }

    public void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
