using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Show one player's color/name/stats as part of horizontal layout group this script is attached to
/// </summary>
public class PlayerDisplay : MonoBehaviour
{
    public Text score;
    public Text name;
    public Image image;

    public MazePlayerUI player;

    //Doesn't make too much sense here, but whatever. This thing will initialize startScreen on the target player
    protected PlayerStartScreen startScreen;

    [SerializeField]
    private GameObject readyText;
    [SerializeField]
    private GameObject notReadyText;

    public bool amReady
    {
        get
        {
            return startScreen.amReady;
        }
    }

    protected void Update()
    {
        //more hackage!
        readyText.SetActive(amReady);
        notReadyText.SetActive(!amReady);
    }

    public Color color
    {
        get
        {
            return image.color;
        }
        set
        {
            image.color = value;
        }
    }

    protected void Start()
    {
        //Colors often update when player's first join, reupdate in a bit
        //SUPER HACK!!! kill me please
        Invoke("Refresh", 1.0f);
    }

    protected bool appIsQuitting = false;
    protected void OnApplicationQuit()
    {
        appIsQuitting = true;
    }

    protected void OnDisable()
    {
        if (!appIsQuitting)
        {
            startScreen.KillSelf();
        }
    }

    protected void Refresh()
    {
        Init(player);
    }

    public void Init(MazePlayerUI player)
    {
        this.player = player;
        startScreen = player.gameObject.AddComponent<PlayerStartScreen>();
        Init((int)player.score.score, player.playerName, player.color);
    }

    protected void Init(int score, string name, Color color)
    {
        this.score.text = score.ToString();
        this.name.text = name;
        this.color = color;
    }
}

