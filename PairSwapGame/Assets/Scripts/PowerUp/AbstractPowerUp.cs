using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPowerUp : MonoBehaviour
{
    static LayerMask layerMask = 6;
    // Start is called before the first frame update
    private bool activated = false;
    public abstract void ApplyPowerUp(Projectile projectile, Vector2 impactDirection);
    protected virtual void TakeOldValues(Projectile oldProj, Projectile newProj)
    {
        newProj.transform.localScale = oldProj.transform.localScale;
        newProj.damage = oldProj.damage;
    }
    private Vector2 GetDirection(Projectile projectile)
    {
        return projectile.rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(activated) return;
        if (collision.gameObject.layer == layerMask)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile))
            {
                activated = true;
                ApplyPowerUp(projectile, GetDirection(projectile));
                Destroy(gameObject);
            }
        }
    }
}
