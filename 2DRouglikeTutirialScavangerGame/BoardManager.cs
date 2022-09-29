using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }

    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] innerWallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void initializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; ++x)
        {
            for (int y = 1; y < rows - 1; ++y)
            {
                gridPositions.Add(new Vector3(x, y, 0));                    // wall, enemy, item을 스폰 가능한 위치들을 만들어줌.
            }
        }
    }

    // OuterWall과 Floor를 깔기 위한 함수.
    void boardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; ++x)
        {
            for (int y = -1; y < rows + 1; ++y)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                Debug.Assert(instance != null);
                instance.transform.SetParent(boardHolder); 
            }
        }

    }

    Vector3 pickRandomPositnion()
    {
        int randomIdx = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIdx];
        gridPositions.RemoveAt(randomIdx);
        return randomPosition;
    }

    void layoutObjectAtRandom(GameObject[] tiles, int min, int max)
    {
        int spawningObjectCount = Random.Range(min, max + 1);
        for (int i = 0; i < spawningObjectCount; ++i)
        {
            Vector3 randomPosition = pickRandomPositnion();
            GameObject tileToSpawn = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tileToSpawn, randomPosition, Quaternion.identity);
        }

    }

    // GameManager에 의해서 호출될 함수
    public void SetupScene(int level)
    {
        boardSetup();
        initializeList();
        layoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        layoutObjectAtRandom(innerWallTiles, wallCount.minimum, wallCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2.0f);
        layoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0.0f), Quaternion.identity);
    }
}
