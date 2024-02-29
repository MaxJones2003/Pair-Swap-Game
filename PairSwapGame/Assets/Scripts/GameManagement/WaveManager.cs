using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    public GameObject[] enemyPrefabs;
    [HideInInspector] public List<AbstractDamageable> spawnedEnemies;
    private int currentWave = 0;
    private int currentHealthGate = 100;
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

        AbstractDamageable enemyScript = Instantiate(enemyPrefabs[(int)enemyEnum], Vector3.zero, Quaternion.identity).GetComponent<AbstractDamageable>();

        int health = PickHealth(enemyEnum);

        enemyScript.SetUp(health);
        TotalEnemyHealth += health;
    }
    public AbstractDamageable PickEnemyToSpawn()
    {
        
        return null;
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
}
public static class RandomExtensions
{   
    public static T NextEnum<T>(this System.Random random) where T : struct, Enum
    {
        var values = (T[])Enum.GetValues(typeof(T));
        return values[random.Next(values.Length)];
    }
}
public enum EEnemyType
{
    Small,
    Big,
    Abductor,
    Dropper
}
