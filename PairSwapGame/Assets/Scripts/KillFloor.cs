using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillFloor : MonoBehaviour
{
    static LayerMask layerMask = 6;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layerMask)
        {
            ObjectPoolManager.ReturnObjectToPool(collision.gameObject, (int)EPoolableObjectType.Projectile, (int)collision.gameObject.GetComponent<Projectile>().projectileType);
        }
    }
}
