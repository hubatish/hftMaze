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

    public void Init()
    {
        foreach(var display in playerDisplays)
        {
            GameObject.Destroy(display.gameObject);
        }
        playerDisplays = new List<PlayerDisplay>();

        for(int i=0; i < PlayerManager.NumberPlayers; i++)
        {
            Debug.Log("got a player");
            MazePlayerUI player = PlayerManager.GetPlayer(i);

            PlayerDisplay display = GameObject.Instantiate(displayPrefab);
            display.GetComponent<RectTransform>().SetParent(displayParent, false);

            display.Init((int)player.score.score, player.playerName,player.color);

            playerDisplays.Add(display);
        }
    }
}

