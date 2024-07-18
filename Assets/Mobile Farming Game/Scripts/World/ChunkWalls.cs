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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(int configuration)
    {
        topWall.SetActive(IsKthBitSet(configuration, 0));
        rightWall.SetActive(IsKthBitSet(configuration, 1));
        bottomWall.SetActive(IsKthBitSet(configuration, 2));
        leftWall.SetActive(IsKthBitSet(configuration, 3));

  //      if(IsKthBitSet(configuration, 0))
  //          topWall.SetActive(true);

		//if (IsKthBitSet(configuration, 1))
		//	rightWall.SetActive(true);

		//if (IsKthBitSet(configuration, 2))
		//	bottomWall.SetActive(true);

		//if (IsKthBitSet(configuration, 3))
		//	leftWall.SetActive(true);
	}

    public bool IsKthBitSet(int configuration, int k)
    {
        if ((configuration & (1 << k)) > 0)
            return false;
        else 
            return true;
    }
}
