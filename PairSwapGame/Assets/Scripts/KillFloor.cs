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
            Destroy(collision.gameObject);
        }
    }
}
