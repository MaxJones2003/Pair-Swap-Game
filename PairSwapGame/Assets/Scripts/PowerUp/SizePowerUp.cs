using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePowerUp : AbstractPowerUp
{
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        projectile.scale *= 1.5f;
        projectile.transform.localScale = projectile.scale;
        projectile.damage *= 2;
    }
}
