using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPowerUp : MonoBehaviour
{
    static LayerMask layerMask = 6;
    // Start is called before the first frame update

    public abstract void ApplyPowerUp(Projectile projectile, Vector2 impactDirection);

    private Vector2 GetDirection(Projectile projectile)
    {
        return projectile.rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            if (collision.gameObject.TryGetComponent(out Projectile projectile))
            {
                
                ApplyPowerUp(projectile, GetDirection(projectile));
                Destroy(gameObject);
            }
        }
    }
}
