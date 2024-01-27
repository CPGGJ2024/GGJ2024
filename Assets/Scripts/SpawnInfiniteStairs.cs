using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInfiniteStairs : MonoBehaviour
{
    public Transform player;
    public static GameObject stair;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(stair, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
