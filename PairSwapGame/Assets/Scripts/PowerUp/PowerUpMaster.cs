using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMaster : MonoBehaviour
{
    public GameObject[] PowerUpPrefabs;
    public static PowerUpMaster Instance;
    private static int PowerUpLength = 0;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            PowerUpLength = PowerUpPrefabs.Length;
        }
        else
            Destroy(gameObject);
    }

    public AbstractPowerUp PickRandomPowerUp(Vector3 position, Quaternion rotation)
    {
        int index = AbstractDamageable.rand.Next(0, PowerUpLength);
        return ObjectPoolManager.SpawnObject(PowerUpPrefabs[index], position, rotation, 2, index).GetComponent<AbstractPowerUp>();
    }
    public AbstractPowerUp SpawnPowerUp(int index, Vector3 position, Quaternion rotation)
    {
        return ObjectPoolManager.SpawnObject(PowerUpPrefabs[index], position, rotation, 2, index).GetComponent<AbstractPowerUp>();
    }
}
