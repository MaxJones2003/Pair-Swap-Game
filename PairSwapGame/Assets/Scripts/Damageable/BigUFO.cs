using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigUFO : AbstractDamageable
{
    [SerializeField, Range(0, 100)] private int spawnChance = 20;
    public override void TakeDamage(int dmg, Vector2 impactDirection)
    {
        Health -= dmg;
        WaveManager.Instance.TotalEnemyHealth -= dmg;
        Jitter(impactDirection);
        if(Health <= 0) Died();
        else SwitchColorIndex(Health);
    }

    protected override void Died()
    {
        if(rand.Next(0, 100) < spawnChance)
        {
            DropPowerup();
        }
        base.Died();
    }
}
