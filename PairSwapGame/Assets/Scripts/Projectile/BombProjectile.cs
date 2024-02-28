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
        
        Grow(-other.relativeVelocity);

    }
    protected override void HitDamageable(AbstractDamageable damageScript, Vector2 hitVelocity)
    {
        base.HitDamageable(damageScript, hitVelocity);
        ReturnToBaseProjectile(hitVelocity);
    }

    private void Grow(Vector2 hitVelocity)
    {
        scale *=1.25f;
        transform.localScale = scale;
        damage += damageGrowthRate;

        hits++;
        if(hits >= numOfHits)
        {
            ReturnToBaseProjectile(hitVelocity);
        }
    }

    private void ReturnToBaseProjectile(Vector3 hitVelocity)
    {
        Projectile newProj = Instantiate(ProjectileMaster.Instance.ProjectilePrefabs[(int)EProjectileType.Basic], 
                    transform.position, 
                    Quaternion.identity).GetComponent<Projectile>();
        newProj.Fire(hitVelocity.normalized);
        Destroy(gameObject);
    }
}
