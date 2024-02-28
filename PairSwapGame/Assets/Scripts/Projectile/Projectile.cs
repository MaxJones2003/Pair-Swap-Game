using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public Rigidbody2D rb;
    [HideInInspector] public Vector3 scale = Vector3.one;
    [SerializeField] private Animator animator;
    private int hitXHash;
    private int hitYHash;
    private int animScaleXHash;
    private int animScaleYHash;
    private int squashTriggerHash;
    //public SpriteRenderer spriteRenderer;
    private const float force = 10;
    private readonly static Vector2 zeroVector = new(0, 0);

    private float lastHitYPosition = 0;
    private readonly static Vector2 slightDownForce = new(0, -0.1f);

    void Start()
    {
        //animator = GetComponent<Animator>();
        // Convert parameter names to hashes
        scale = Vector3.one;
        hitXHash = Animator.StringToHash("hitX");
        hitYHash = Animator.StringToHash("hitY");
        animScaleXHash = Animator.StringToHash("animScaleX");
        animScaleYHash = Animator.StringToHash("animScaleY");
        squashTriggerHash = Animator.StringToHash("Squash");
    }
    void LateUpdate()
    {
        Vector2 newScale = scale;
        newScale.x *= animator.GetFloat(animScaleXHash);
        newScale.y *= animator.GetFloat(animScaleYHash);
        transform.localScale = newScale;
    }
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
        #region Animation
            Vector2 hitNormal = collision.contacts[0].normal;

            // Calculate the parameters for the blend tree
            float hitX = hitNormal.x;
            float hitY = hitNormal.y;

            // Set the parameters on the animator using hashes
            animator.SetFloat(hitXHash, hitX);
            animator.SetFloat(hitYHash, hitY);

            // Trigger the squash animation using the hash
            animator.SetTrigger(squashTriggerHash);
        #endregion

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
    private const float squishAmount = 0.5f;
    private const float squishDuration = 0.2f;

    #endregion
    IEnumerator SquishCoroutine(Vector2 hitNormal)
    {
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 squishScale = originalScale - new Vector3(hitNormal.x, hitNormal.y, 0) * squishAmount;

        while (elapsed < squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / squishDuration;
            transform.localScale = Vector3.Lerp(originalScale, squishScale, t);
            yield return null;
        }

        // Optionally, return the ball to its original size over time
        elapsed = 0f;
        while (elapsed < squishDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / squishDuration;
            transform.localScale = Vector3.Lerp(squishScale, originalScale, t);
            yield return null;
        }
    }
}
