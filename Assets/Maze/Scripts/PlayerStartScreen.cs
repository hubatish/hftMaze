using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Disable the player while the start screen is up
///     This script attaches to player. Does take input for start ready function though
/// </summary>
public class PlayerStartScreen : ZBehaviour
{
    public bool amReady = false;

    protected void Start()
    {
        Cached<MazePlayerMovement>().enabled = false;
        Cached<PlayerVisibility>().visible = false;
        Cached<PlayerVisibility>().enabled = false;
        Cached<CircleCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get left/right input (get both phone and local input)
        float vMove = Cached<HFTInput>().GetAxis("Vertical") + Input.GetAxis("Vertical");

        float minMove = 0.05f;

        if (Mathf.Abs(vMove) > minMove)
        {
            //Press up to be ready
            amReady = (vMove < 0);
        }
    }

    public void KillSelf()
    {
        Cached<MazePlayerMovement>().enabled = true;
        Cached<PlayerVisibility>().visible = true;
        Cached<PlayerVisibility>().enabled = true;
        Cached<CircleCollider2D>().enabled = true;
        GameObject.Destroy(this);
    }
}

