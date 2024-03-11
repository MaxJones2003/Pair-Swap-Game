using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    [SerializeField] private AbductorUFO UFO;
    [SerializeField] private Transform destroyTarget;
    [SerializeField] private LayerMask layerMask;
    [HideInInspector] public Rigidbody2D currentRigidbody;
    public Coroutine Abduct;

    [SerializeField] private Transform target;
    public float slowDownRate = 0.15f; // How much to reduce the velocity each frame
    public float lerpSpeed = 0.05f; // Speed of the lerp movement
    bool Abducting => Abduct != null;
    private static float radius = 1;
    private static Vector2 up = Vector2.up;

    void Update()
    {
        if(Abducting)
        {
            Debug.Log("buggin");
            CheckForDestruction();
        }
    }
    void CheckForDestruction()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(destroyTarget.position, radius, up, layerMask);
        int len = hits.Length;
        for(int i = 0; i < len; i++)
            if(hits[i].transform.TryGetComponent(out Projectile projectile))
            {
                ObjectPoolManager.ReturnObjectToPool(hits[i].transform.gameObject, (int)EPoolableObjectType.Projectile, (int)projectile.projectileType);
                UFO.SpawnEnemyProjectiles();
                UFO.Die();
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 6 /*Ball layer*/ && other.TryGetComponent(out Projectile projectile))
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
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(destroyTarget.position, radius);
    }
}
