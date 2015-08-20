using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Show all of the players' color/name/stats in horizontal layout group this script is attached to
///     Manage adding/deleting of those players' displays as they come & go
///     Check when all are ready to see if they're good to go
///     Could also be called StartAndEndScreen or TheMenu
/// </summary>
public class PlayerGridDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerDisplay displayPrefab;

    [SerializeField]
    private List<PlayerDisplay> playerDisplays = new List<PlayerDisplay>();

    [SerializeField]
    private RectTransform displayParent;

    public Text titleText;

    public Action onReadyEvent = delegate () {};

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
        if (playerDisplays.Count > 1 && allReady)
        {
            //Won't this get called a lot?
            //  Usually, but here the onReadyEvent of RoundMangaer will be turning this script off
            onReadyEvent();
        }
    }

    protected void OnEnable()
    {
        if (displayParent == null)
        {
            displayParent = gameObject.GetComponent<RectTransform>();
        }
        Init();

        PlayerManager.playerAddEvent += AddPlayer;
        PlayerManager.playerRemoveEvent += RemovePlayer;
    }

    protected void OnDisable()
    {
        PlayerManager.playerAddEvent -= AddPlayer;
        PlayerManager.playerRemoveEvent -= RemovePlayer;
        ClearAllPlayers();
    }

    protected void ClearAllPlayers()
    {
        foreach (var display in playerDisplays)
        {
            GameObject.Destroy(display.gameObject);
        }
        playerDisplays = new List<PlayerDisplay>();
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
        ClearAllPlayers();

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

