using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingProjectile : MonoBehaviour
{
    [SerializeField] private GameObject startingProjectile;
    void Start()
    {
        ObjectPoolManager.SpawnObject(startingProjectile, transform.position, 
            Quaternion.identity, (int)EPoolableObjectType.Projectile, 
            (int)EProjectileType.Basic).GetComponent<Projectile>().Fire(Vector2.up);
    }
}
