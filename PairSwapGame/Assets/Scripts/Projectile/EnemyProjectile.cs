using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        /* float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);
        Color c = new(x, y, z);
        spriteRenderer.color = c; */

        if(collision.transform.TryGetComponent(out Projectile projectile))
        {
            int projInt = (int)EPoolableObjectType.Projectile;
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject, projInt, (int)projectile.projectileType);
            ObjectPoolManager.ReturnObjectToPool(gameObject, projInt, (int)projectileType);
        }
        else
            StartCoroutine(SquishCoroutine(-collision.relativeVelocity));
        

        float lastY = transform.position.y;
        if(lastHitYPosition == lastY)
        {
            rb.AddForce(slightDownForce, ForceMode2D.Impulse);
        }
        lastHitYPosition = lastY;
    }

    protected override void OnEnable()
    {
        
    }
    protected override void OnDisable()
    {
        
    }
}
