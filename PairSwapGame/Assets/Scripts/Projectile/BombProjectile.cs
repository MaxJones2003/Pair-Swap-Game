using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    public int numOfHits = 5;
    public int damageGrowthRate = 2;

    private int hits = 0;

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        
        Grow();

    }
    protected override void HitDamageable(AbstractDamageable damageScript, Vector2 hitVelocity)
    {
        base.HitDamageable(damageScript, hitVelocity);
    }

    private void Grow()
    {
        transform.localScale *= 1.25f;
        damage += damageGrowthRate;

        hits++;
        if(hits >= numOfHits)
            Destroy(gameObject);
    }
}
