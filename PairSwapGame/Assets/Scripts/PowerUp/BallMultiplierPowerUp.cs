using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMultiplierPowerUp : AbstractPowerUp
{
    public GameObject testPrefab;
    const float spawnDistModifier = 1.5f;
    const float dirForceModifier = 2f;
    public override void ApplyPowerUp(Projectile projectile, Vector2 impactDirection)
    {
        int r = 1;
        for(int i = 0; i < 2; i++)
        {
            Vector2 right = (Vector2.right * r);
           
            GameObject pref = Instantiate(testPrefab, projectile.transform.position + (new Vector3(right.x, right.y, 0) * spawnDistModifier), Quaternion.identity);
            Projectile newProj = pref.GetComponent<Projectile>();
            Vector2 newDir = impactDirection + right * dirForceModifier;
            r *= -1;
            newProj.Fire(newDir.normalized);

        }
        Destroy(gameObject);

    }
}
