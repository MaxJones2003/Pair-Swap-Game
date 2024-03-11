using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public GameObject[] enemyPrefabs;
    [HideInInspector] public List<AbstractDamageable> spawnedEnemies;
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
            if(totalEnemyHealth < 0) totalEnemyHealth = 0;
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
    public void SpawnEnemy(EEnemyType enemyEnum)
    {
        int enemyEnumValue = (int)enemyEnum;
        AbstractDamageable enemyScript = ObjectPoolManager.SpawnObject(enemyPrefabs[enemyEnumValue], leftOrRight ? spawnLeft : spawnRight, Quaternion.identity,
                                                            (int)EPoolableObjectType.Enemy, enemyEnumValue).GetComponent<AbstractDamageable>();
        leftOrRight = !leftOrRight;
        int health = PickHealth(enemyEnum);
        Vector2 finalPos = SelectTargetPos();
        enemyScript.SetUp(health, finalPos);
        TotalEnemyHealth += health;
        currentHealthGate++;
        difficulty *= 1.01f;
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
        for (int i = 0; i < len; i++)
        {
            plannedEnemies[i] = GetRandomWeightedEnemyType();
        }
        enemyIndex = 0;
    }

    private EEnemyType GetRandomWeightedEnemyType()
    {
        var values = (EEnemyType[])Enum.GetValues(typeof(EEnemyType));
        var weights = values.Select(v => v.GetWeight()).ToArray();
        var cumulativeDistribution = new int[weights.Length];
        int totalWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
            cumulativeDistribution[i] = totalWeight;
        }

        int randomValue = random.Next(totalWeight);
        int selectedIndex = Array.FindIndex(cumulativeDistribution, x => x >= randomValue);

        return values[selectedIndex];
    }
    private static readonly Vector2 Size = new(0.5f, 0.5f);
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(spawnLeft, Size);
        Gizmos.DrawCube(spawnRight, Size);
    }
}
public static class EnemyTypeExtensions
{
    public static int GetWeight(this EEnemyType enemyType)
    {
        switch (enemyType)
        {
            case EEnemyType.Small:
                return 10;
            case EEnemyType.Big:
                return 7; 
            case EEnemyType.Abductor:
                return 3;
            case EEnemyType.Dropper:
                return 3; 
            default:
                return 1; // Default weight
        }
    }
}
/* public static class RandomExtensions
{   
    public static T NextEnum<T>(this System.Random random) where T : struct, Enum
    {
        var values = (T[])Enum.GetValues(typeof(T));
        return values[random.Next(values.Length)];
    }
} */
