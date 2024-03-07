using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D currentRigidbody;
    public Coroutine Abduct;

    [SerializeField] private Transform target;
    public float slowDownRate = 0.15f; // How much to reduce the velocity each frame
    public float lerpSpeed = 0.05f; // Speed of the lerp movement

    void Start()
    {
        // Initialization if needed
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Projectile projectile))
        {
            StartAbduction(projectile);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent(out Projectile projectile))
        {
            if (currentRigidbody == projectile.rb)
            {
                StopCoroutine(Abduct);
                Abduct = null;
                currentRigidbody = null;
                projectile.Fire(projectile.rb.velocity.normalized);
            }
        }
    }

    private void StartAbduction(Projectile projectile)
    {
        currentRigidbody = projectile.rb;

        if(Abduct == null)
        {
            Abduct = StartCoroutine(AbductProjectile());
        }
    }

    private IEnumerator AbductProjectile()
    {
        while(true)
        {
            // Slow down the velocity
            if (currentRigidbody.velocity.sqrMagnitude > 2)
            {
                currentRigidbody.velocity -= currentRigidbody.velocity.normalized * slowDownRate;
            }
            else
            {
                // Once the velocity reaches 0, lerp the position
                currentRigidbody.position = Vector2.Lerp(currentRigidbody.position, target.position, lerpSpeed);
            }
            yield return null;
        }
    }
}
