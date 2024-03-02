using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMaster : MonoBehaviour
{
    public GameObject[] ProjectilePrefabs;
    public static ProjectileMaster Instance;


    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
