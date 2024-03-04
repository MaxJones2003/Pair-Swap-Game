using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : AbstractPowerUp
{
    public GameObject BombPrefab;
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        BombProjectile newProj = ObjectPoolManager.SpawnObject(BombPrefab, projectile.transform.position, Quaternion.identity, (int)EPoolableObjectType.Projectile, (int)EProjectileType.Bomb).GetComponent<BombProjectile>();
        TakeOldValues(projectile, newProj);
        newProj.SetupPreviousProjectile(projectile.projectileType, projectile.transform.localScale, projectile.damage);
        newProj.Fire(impactDirection.normalized);
        ObjectPoolManager.ReturnObjectToPool(projectile.gameObject, (int)EPoolableObjectType.Projectile, (int)projectile.projectileType);
    }
}
