﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ZachUtility;

public class PlayerVisibility : ZBehaviour
{
    public bool visible
    {
        set
        {
            Cached<SpriteRenderer>().enabled = value;
        }
        get
        {
            return Cached<SpriteRenderer>().enabled;
        }
    }

    //Time until we turn visible
    private Timer turnVisibleTimer = new Timer();
    //Time until turning invisible
    private Timer turnInvisibleTimer = new Timer();

    // Use this for initialization
    void Start()
    {
        turnInvisibleTimer.Done += BlinkOff;

        turnVisibleTimer.Done += BlinkOn;

        turnInvisibleTimer.Start(visibleTime);

        Cached<MazePlayerMovement>().onBlockHit += BlinkOn;
    }

    void Update()
    {
        turnInvisibleTimer.Update();
        turnVisibleTimer.Update();
    }

    [Tooltip("Time to stay visible")]
    public float visibleTime = 1f;
    [Tooltip("Time to stay invisible")]
    public float inVisibleTime = 5f;

    public void PermanentOn()
    {
        turnInvisibleTimer.on = false;
        turnVisibleTimer.on = false;
        visible = true;
    }

    public void PermanentOff()
    {
        turnVisibleTimer.on = false;
        turnInvisibleTimer.on = false;
        visible = true;
    }

    public void BlinkOn()
    {
        visible = true;
        turnVisibleTimer.on = false;
        turnInvisibleTimer.Start(visibleTime);
    }

    public void BlinkOff()
    {
        visible = false;
        turnInvisibleTimer.on = false;
        turnVisibleTimer.Start(inVisibleTime);
    }

}
