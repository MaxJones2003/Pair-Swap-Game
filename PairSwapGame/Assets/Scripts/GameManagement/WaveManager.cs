using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public GameObject[] enemyPrefabs;
    [HideInInspector] public List<AbstractDamageable> spawnedEnemies;
    private int currentWave = 0;
    [SerializeField] private int currentHealthGate = 100;
    [SerializeField] private int totalEnemyHealth = 0;
    public int TotalEnemyHealth
    {
        get
        {
            return totalEnemyHealth;
        }
        set
        {
            totalEnemyHealth = value;
            if(totalEnemyHealth < currentHealthGate)
            {
                SpawnEnemy();
            }
        }
    }

    private float difficulty = 1;

    private EEnemyType[] plannedEnemies = new EEnemyType[10];
    private int enemyIndex = 0;

    [SerializeField] private Vector2 spawnRight, spawnLeft;
    private bool leftOrRight;
    Bounds bounds;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy any duplicates
        }
        bounds = GetComponent<BoxCollider2D>().bounds;

        SetNextEnemies();
        SpawnEnemy();
    }
    // spawn enemies based off of current total health of all enemies in the level
    // this way we can spawn a ton of low health enemies or a few high health ones

    public void SpawnEnemy()
    {
        if(enemyIndex >= plannedEnemies.Length)
            SetNextEnemies();
        EEnemyType enemyEnum = plannedEnemies[enemyIndex++];
        int enemyEnumValue = (int)enemyEnum;
        AbstractDamageable enemyScript = ObjectPoolManager.SpawnObject(enemyPrefabs[enemyEnumValue], leftOrRight ? spawnLeft : spawnRight, Quaternion.identity,
                                                            (int)EPoolableObjectType.Enemy, enemyEnumValue).GetComponent<AbstractDamageable>();
        leftOrRight = !leftOrRight;
        int health = PickHealth(enemyEnum);
        Vector2 finalPos = SelectTargetPos();
        enemyScript.SetUp(health, finalPos);
        TotalEnemyHealth += health;
    }

    private Vector2 SelectTargetPos()
    {
        // Calculate the random position within the bounds
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        // Convert the local coordinates to world position
        Vector2 worldPosition = new Vector2(randomX, randomY);

        return worldPosition;
    }


    public int PickHealth(EEnemyType enemyType)
    {
        return enemyType switch
        {
            EEnemyType.Small => Mathf.FloorToInt(5f * difficulty),
            EEnemyType.Big => Mathf.FloorToInt(15f * difficulty),
            EEnemyType.Abductor => Mathf.FloorToInt(25f * difficulty),
            EEnemyType.Dropper => Mathf.FloorToInt(35f * difficulty),
            _ => 0,
        };
    }    

    System.Random random = new System.Random();
    private void SetNextEnemies()
    {
        int len = plannedEnemies.Length;
        for(int i = 0; i < len; i++)
        {
            plannedEnemies[i] = random.NextEnum<EEnemyType>();
        }
        enemyIndex = 0;
    }
    private static readonly Vector2 Size = new(0.5f, 0.5f);
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(spawnLeft, Size);
        Gizmos.DrawCube(spawnRight, Size);
    }
}
public static class RandomExtensions
{   
    public static T NextEnum<T>(this System.Random random) where T : struct, Enum
    {
        var values = (T[])Enum.GetValues(typeof(T));
        return values[random.Next(values.Length)];
    }
}
