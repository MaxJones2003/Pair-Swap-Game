using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : AbstractPowerUp
{
    public GameObject BombPrefab;
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        Projectile newProj = Instantiate(BombPrefab, projectile.transform.position, Quaternion.identity).GetComponent<Projectile>();
        TakeOldValues(projectile, newProj);

        newProj.Fire(impactDirection.normalized);
        Destroy(projectile.gameObject);
    }
}