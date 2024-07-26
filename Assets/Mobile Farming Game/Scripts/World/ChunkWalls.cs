using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;

    public void Configure(int configuration)
    {
        topWall.SetActive(IsKthBitSet(configuration, 0));
        rightWall.SetActive(IsKthBitSet(configuration, 1));
        bottomWall.SetActive(IsKthBitSet(configuration, 2));
        leftWall.SetActive(IsKthBitSet(configuration, 3));
	}

    public bool IsKthBitSet(int configuration, int k)
    {
        if ((configuration & (1 << k)) > 0)
            return false;
        else 
            return true;
    }
}
