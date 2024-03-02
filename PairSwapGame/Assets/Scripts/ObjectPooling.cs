using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    private static Transform _objectPoolEmptyHolder;
    private static Transform _enemyParent;
    private static Transform _projectileParent;

    private void Awake() {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects").transform;

        _enemyParent = new GameObject("Enemies").transform;
        _projectileParent = new GameObject("Projectiles").transform;

        _enemyParent.SetParent(_objectPoolEmptyHolder);
        _projectileParent.SetParent(_objectPoolEmptyHolder);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, int objectType, int subType)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.ObjectType == objectType && p.SubType == subType);

        if(pool == null)
        {
            pool = new PooledObjectInfo() { ObjectType = objectType, SubType = subType };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObj.transform.SetParent(objectType == 0 ? _projectileParent : objectType == 1 ? _enemyParent : _objectPoolEmptyHolder); // choose between the enemy, projectile, and default parent
        }
        else
        {
            spawnableObj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj, int objectType, int subType)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.ObjectType == objectType && p.SubType == subType);

        if(pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public int ObjectType;
    public int SubType;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}

public enum EPoolableObjectType
{
    Projectile,
    Enemy
}

