using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    private const float force = 10;
    private readonly static Vector2 zeroVector = new(0, 0);

    public void Fire(Vector2 direction)
    {
        rb.velocity = zeroVector;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
    public void AddVelocity(Vector2 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
        Vector2 vel = rb.velocity.normalized;
        rb.velocity = vel * force;
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);
        Color c = new(x, y, z);

        spriteRenderer.color = c;
        if(collision.transform.TryGetComponent(out AbstractDamageable damageScript))
        {
            HitDamageable(damageScript, -collision.relativeVelocity);
        }
    }

    protected virtual void HitDamageable(AbstractDamageable damageScript, Vector2 hitVelocity)
    {
        damageScript.TakeDamage(damage, hitVelocity);
    }
}
