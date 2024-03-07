using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Projectile projectile))
        {
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject, (int)EPoolableObjectType.Projectile, (int)projectile.projectileType);
        }
    }
}
