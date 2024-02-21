using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingProjectile : MonoBehaviour
{
    void Start()
    {
        GetComponent<Projectile>().PewPew(Vector2.up);
    }
}
