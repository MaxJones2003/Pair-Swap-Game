using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    public int numOfHits = 5;
    public int damageGrowthRate = 2;

    private int hits = 0;
    private ProjectileInfo previousProjectileInfo;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out AbstractDamageable damageScript))
        {
            HitDamageable(damageScript, -collision.relativeVelocity);
            return;
        }

        StartCoroutine(SquishCoroutine(-collision.relativeVelocity));

        float lastY = transform.position.y;
        if (lastHitYPosition == lastY)
        {
            rb.AddForce(slightDownForce, ForceMode2D.Impulse);
        }
        lastHitYPosition = lastY;

        Grow(-collision.relativeVelocity);
    }
    protected override void HitDamageable(AbstractDamageable damageScript, Vector2 hitVelocity)
    {
        base.HitDamageable(damageScript, hitVelocity);
        ReturnToBaseProjectile(hitVelocity);
    }

    private void Grow(Vector2 hitVelocity)
    {
        transform.localScale *= 1.5f;
        damage += damageGrowthRate;

        hits++;
        if(hits >= numOfHits)
        {
            ReturnToBaseProjectile(hitVelocity);
        }
    }

    private void ReturnToBaseProjectile(Vector3 hitVelocity)
    {
        Projectile newProj = ObjectPoolManager.SpawnObject(ProjectileMaster.Instance.ProjectilePrefabs[(int)EProjectileType.Basic], 
                    transform.position, Quaternion.identity,
                    (int)EPoolableObjectType.Projectile, (int)previousProjectileInfo.Type).GetComponent<Projectile>();
        newProj.Fire(hitVelocity.normalized);
        ObjectPoolManager.ReturnObjectToPool(gameObject, (int)EPoolableObjectType.Projectile, (int)EProjectileType.Bomb);
    }

    public void SetupPreviousProjectile(EProjectileType type, Vector2 scale, int damage)
    {
        previousProjectileInfo = new ProjectileInfo(type, scale, damage);
    }
}
