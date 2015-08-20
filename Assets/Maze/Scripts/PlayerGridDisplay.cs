using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Show player's color/name/stats in horizontal layout group this script is attached to
/// </summary>
public class PlayerGridDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerDisplay displayPrefab;

    [SerializeField]
    private List<PlayerDisplay> playerDisplays = new List<PlayerDisplay>();

    [SerializeField]
    private RectTransform displayParent;

    protected void Start()
    {
        if (displayParent == null)
        {
            displayParent = gameObject.GetComponent<RectTransform>();
        }
        Init();
    }

    protected void Update()
    {
        bool allReady = true;
        foreach(PlayerDisplay display in playerDisplays)
        {
            if (!display.amReady)
            {
                allReady = false;
            }
        }
        if (allReady)
        {
            Debug.Log("all are ready");
        }
    }

    protected void OnEnable()
    {
        PlayerManager.playerAddEvent += AddPlayer;
        PlayerManager.playerRemoveEvent += RemovePlayer;
    }

    protected void OnDisable()
    {
        PlayerManager.playerAddEvent -= AddPlayer;
        PlayerManager.playerRemoveEvent -= RemovePlayer;
    }

    protected void AddPlayer(MazePlayerUI player)
    {
        PlayerDisplay display = GameObject.Instantiate(displayPrefab);
        display.GetComponent<RectTransform>().SetParent(displayParent, false);
        display.Init(player);
        playerDisplays.Add(display);
    }

    public void Init()
    {
        foreach(var display in playerDisplays)
        {
            GameObject.Destroy(display.gameObject);
        }
        playerDisplays = new List<PlayerDisplay>();

        for(int i=0; i < PlayerManager.NumberPlayers; i++)
        {
            MazePlayerUI player = PlayerManager.GetPlayer(i);
            AddPlayer(player);
        }
    }

    protected void RemovePlayer(MazePlayerUI player)
    {
        int pIndex = playerDisplays.FindIndex(display => display.player == player);
        GameObject.Destroy(playerDisplays[pIndex].gameObject);
        playerDisplays.RemoveAt(pIndex);
    }
}

