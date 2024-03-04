using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingProjectile : MonoBehaviour
{
    [SerializeField] private GameObject startingProjectile;
    void Start()
    {
        Projectile proj = ObjectPoolManager.SpawnObject(startingProjectile, transform.position, 
            Quaternion.identity, (int)EPoolableObjectType.Projectile, 
            (int)EProjectileType.Basic).GetComponent<Projectile>();
        proj.Setup(Projectile.DefaultProjectileInfo);
        proj.Fire(Vector2.up);

    }
}
