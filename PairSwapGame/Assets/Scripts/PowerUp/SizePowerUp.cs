using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePowerUp : AbstractPowerUp
{
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        projectile.Info.Scale *= 1.5f;
        projectile.transform.localScale = projectile.Info.Scale;
        projectile.damage *= 2;
    }
}
