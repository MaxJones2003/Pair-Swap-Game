using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePowerUp : AbstractPowerUp
{
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        projectile.transform.localScale *= 1.5f;
    }
}
