using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class WaveSpawner : MonoBehaviour
{
    public enum eSpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int enemyCount;
        public float rate;

    }
    public Wave[] waves;
    private int mNextWaveIdx = 0;
    public int NextWaveIndx { get { return mNextWaveIdx; } }

    public Transform[] spawnPoints;

    public float timeBetweenWave = 3.0f;
    private float mWaveCountdown;
    public float WaveCountdown { get { return mWaveCountdown; } }

    private float enemySearchCountdownInterval = 1.0f;
    private float enemySearchCountdown = 1.0f;

    private eSpawnState mState = eSpawnState.COUNTING;
    public eSpawnState State { get { return mState; } }

    void Start()
    {
        Debug.Assert(spawnPoints.Length != 0);
        mWaveCountdown = timeBetweenWave;
    }

    void Update()
    {
        if (mState == eSpawnState.WAITING)
        {
            if (!isAnyEnemyAlive())
            {
                // Begin a New Round
                mState = eSpawnState.COUNTING;
                mWaveCountdown = timeBetweenWave;

                if (mNextWaveIdx + 1 >= waves.Length)
                {
                    mNextWaveIdx = 0;
                    return;
                }

                ++mNextWaveIdx;
            }
            else
            {
                return;
            }
        }

        if (mWaveCountdown <= 0f)
        {
            if (mState != eSpawnState.SPAWNING)
            {
                StartCoroutine(spawnWave(waves[mNextWaveIdx]));
            }
        }
        else
        {
            mWaveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator spawnWave(Wave wave)
    {
        mState = eSpawnState.SPAWNING;

        for (int i = 0; i < wave.enemyCount; ++i)
        {
            spawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1.0f / wave.rate);
        }

        mState = eSpawnState.WAITING;
        yield break;
    }

    void spawnEnemy(Transform enemy)
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, sp.position, sp.rotation);
    }

    private bool isAnyEnemyAlive()
    {
        enemySearchCountdown -= Time.deltaTime;
        if (enemySearchCountdown <= 0f)
        {
            enemySearchCountdown = enemySearchCountdownInterval;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

}
