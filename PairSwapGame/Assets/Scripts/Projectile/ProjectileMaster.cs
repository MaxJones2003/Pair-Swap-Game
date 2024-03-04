using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileMaster : MonoBehaviour
{
    public GameObject[] ProjectilePrefabs;
    public static ProjectileMaster Instance;

    private int _currentNumOfProjectiles = 0;
    public int CurrentNumOfProjectiles
    {
        get
        {
            return _currentNumOfProjectiles;
        }
        set
        {
            if(value <= 0)
            {
                OutOfProjectiles();
            }
            _currentNumOfProjectiles = value;
        }
    }

    private void OutOfProjectiles()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        _currentNumOfProjectiles = 0;
    }
}
