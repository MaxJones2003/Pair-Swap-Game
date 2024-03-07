using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileInfo
{
    private static readonly Vector2 MaxSize = new(4, 4);
    public EProjectileType Type;
    private Vector2 _scale;
    public Vector2 Scale {
        get
        {
            return _scale;
        }
        set 
        {
            if(value.sqrMagnitude > MaxSize.sqrMagnitude)
                _scale = MaxSize;
            else
                _scale = value;
        }
    }
    public int Damage;

    public ProjectileInfo(EProjectileType Type, Vector2 Scale, int Damage)
    {
        this.Type = Type;
        _scale = Scale;
        this.Damage = Damage;
    }
}
