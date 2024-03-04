using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMultiplierPowerUp : AbstractPowerUp
{
    public GameObject testPrefab;
    const float spawnDistModifier = 1.5f;
    const float dirForceModifier = 2f;
    private static Vector2 Right = new(1, 0);
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        int r = 1;
        Vector2 right = Right * r;
        for(int i = 0; i < 2; i++)
        {
            GameObject pref = ObjectPoolManager.SpawnObject(testPrefab, projectile.transform.position + ((Vector3)right * spawnDistModifier), 
                Quaternion.identity, (int)EPoolableObjectType.Projectile, (int)projectile.projectileType);
                
            Projectile newProj = pref.GetComponent<Projectile>();
            newProj.Setup(Projectile.DefaultProjectileInfo);
            Vector2 newDir = (impactDirection + right) * dirForceModifier;
            newProj.Fire(newDir.normalized);

            r *= -1;
            right = Right * r;
        }
        Destroy(gameObject);
    }
}
