using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : AbstractPowerUp
{
    public GameObject BombPrefab;
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        Vector2 vel = projectile.rb.velocity;
        Projectile newProj = Instantiate(BombPrefab, projectile.transform.position, Quaternion.identity).GetComponent<Projectile>();
        Destroy(projectile.gameObject);

        newProj.rb.velocity = vel;
    }
}
