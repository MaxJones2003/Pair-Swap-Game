using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileInfo
{
    public EProjectileType Type;
    public Vector2 Scale;
    public int Damage;

    public ProjectileInfo(EProjectileType Type, Vector2 Scale, int Damage)
    {
        this.Type = Type;
        this.Scale = Scale;
        this.Damage = Damage;
    }
}
