using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject fishPrefab;

    [SerializeField]
    private int numberFish = 5;

    protected GridManager grid;

    protected void Start()
    {
        grid = GridManager.Instance;
        SpawnLotsFish();
    }

    protected void SpawnLotsFish()
    {
        for(int i = 0; i < numberFish; i++)
        {
            SpawnFish();
        }
    }

    // Warning: Cause infinite loop if all positions in grid occupied
    protected void SpawnFish()
    {
        int x = UnityEngine.Random.Range(0, grid.numColumns);
        int y = UnityEngine.Random.Range(0, grid.numRows);
        Vector3 pos = grid.ijToxyz(new GridVector(x, y));
        if(grid.GetHitsAtPos(pos).Count() == 0)
        {
            GameObject.Instantiate(fishPrefab, pos, Quaternion.identity);
        }
        else
        {
            SpawnFish();
        }
    }
}
