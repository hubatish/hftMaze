using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject fishPrefab;

    private int numExtraFish = -1;

    protected GridManager grid;

    protected void Start()
    {
        grid = GridManager.Instance;
    }

    private bool finished = true;

    protected void Update()
    {
        if (!finished && transform.childCount==0)
        {
            finished = true;
            Debug.Log("Just finished!");
            RoundManager.Instance.EndGame(false);
        }
    }

    public void SpawnLotsFish()
    {
        finished = false;

        //Clear previous fish
        for(int i = transform.childCount-1; i>=0;  i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        int numberFish = numExtraFish + PlayerManager.NumberPlayers;
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
            Transform newFish = ((GameObject) GameObject.Instantiate(fishPrefab, pos, Quaternion.identity)).transform;
            newFish.SetParent(transform,true);
        }
        else
        {
            SpawnFish();
        }
    }
}
