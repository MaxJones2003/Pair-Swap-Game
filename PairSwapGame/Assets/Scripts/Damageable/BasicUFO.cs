using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUFO : AbstractDamageable
{
    public override void TakeDamage(int dmg, Vector2 impactDirection)
    {
        Health -= dmg;
        WaveManager.Instance.TotalEnemyHealth -= dmg;
        Jitter(impactDirection);
        if(Health <= 0) Died();
    }

    public override void Died()
    {
        Destroy(gameObject);
    }
}