using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public Rigidbody2D rb;
    public EProjectileType projectileType;
    [SerializeField] private Transform sprite;

    private const float force = 10;
    private readonly static Vector2 zeroVector = new(0, 0);
    private readonly static Vector2 oneVector = new(1, 1);

    private float lastHitYPosition = 0;
    private readonly static Vector2 slightDownForce = new(0, -0.1f);

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
        /* float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);
        Color c = new(x, y, z);
        spriteRenderer.color = c; */

        if(collision.transform.TryGetComponent(out AbstractDamageable damageScript))
        {
            HitDamageable(damageScript, -collision.relativeVelocity);
        }
        
        StartCoroutine(SquishCoroutine(-collision.relativeVelocity));

        float lastY = transform.position.y;
        if(lastHitYPosition == lastY)
        {
            rb.AddForce(slightDownForce, ForceMode2D.Impulse);
        }
        lastHitYPosition = lastY;
    }

    protected virtual void HitDamageable(AbstractDamageable damageScript, Vector2 hitVelocity)
    {
        damageScript.TakeDamage(damage, hitVelocity);
    }

    #region Squish
    private const float squishAmount = 0.1f;
    private const float squishDuration = 0.1f;

    IEnumerator SquishCoroutine(Vector2 hitNormal)
    {
        //float x = Mathf.Abs(hitNormal.x);
        // hitNormal.x = Mathf.Abs(hitNormal.y);
        //hitNormal.y = x;
        hitNormal.x = Mathf.Abs(hitNormal.x);
        hitNormal.y = Mathf.Abs(hitNormal.y);
        float elapsed = 0f;
        Vector3 originalScale = oneVector;
        Vector3 squishScale = originalScale - (Vector3)hitNormal * squishAmount;

        while (elapsed < squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / squishDuration;
            sprite.localScale = Vector3.Lerp(originalScale, squishScale, t);
            yield return null;
        }

        // Optionally, return the ball to its original size over time
        elapsed = 0f;
        while (elapsed < squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / squishDuration;
            sprite.localScale = Vector3.Lerp(squishScale, originalScale, t);
            yield return null;
        }
    }
    #endregion



    void OnEnable()
    {
        ProjectileMaster.Instance.CurrentNumOfProjectiles++;
    }
    void OnDisable()
    {
        ProjectileMaster.Instance.CurrentNumOfProjectiles--;
    }
}
