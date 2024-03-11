using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorbeamDestroyer : MonoBehaviour
{
    [SerializeField] private AbductorUFO UFO;
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == 6 /*Ball layer*/ && other.gameObject.TryGetComponent(out Projectile projectile))
        {
            Debug.Log("Destroyer triggered");
            ObjectPoolManager.ReturnObjectToPool(other.gameObject, (int)EPoolableObjectType.Projectile, (int)projectile.projectileType);
            UFO.SpawnEnemyProjectiles();
            UFO.Die();
        }
    }
}
