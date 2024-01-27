using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnInfiniteStairs : MonoBehaviour
{
    public Transform player;
    public GameObject stair;
    public Vector3 initialSpawnLoc;
    public float stairWidthOffset;
    public float stairHeightOffset;
    public int initialStairs = 10;
    public int maxSpawnedStairs = 60;

    private Vector3 curSpawnLoc;
    private Queue<GameObject> stairQueue = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        curSpawnLoc = initialSpawnLoc;
        for (int i = 0; i < initialStairs; i++)
        {
            GameObject newStair = Instantiate(stair, curSpawnLoc, Quaternion.identity);
            stairQueue.Enqueue(newStair);
            curSpawnLoc += new Vector3(stairWidthOffset, stairHeightOffset * -1, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        while(MathF.Abs(player.position.y - curSpawnLoc.y) < 20)
        {
            GameObject newStair = Instantiate(stair, curSpawnLoc, Quaternion.identity);
            stairQueue.Enqueue(newStair);
            curSpawnLoc += new Vector3(stairWidthOffset, stairHeightOffset * -1, 0f);
        }
        while(stairQueue.Count > maxSpawnedStairs)
        {
            Destroy(stairQueue.Dequeue());
        }
    }
}
