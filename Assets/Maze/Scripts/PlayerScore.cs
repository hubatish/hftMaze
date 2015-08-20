using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class PlayerScore
{
    public float score = 0;
    public bool chasing;

    public Action caughtEvent = delegate () { };
    public Action catchPlayerEvent = delegate () { };

    public void GetCaught()
    {
        //chasing = !chasing;
        caughtEvent();
    }

    public void CatchPlayer()
    {
        //chasing = !chasing;
        score += 1;
        catchPlayerEvent();
    }

    public void Update()
    {
    }

    public override string ToString()
    {
        string s;
        if (chasing)
        {
            s = "Seeker";
        }
        else
        {
            s = "Hider";
        }
        return s + ((int)score).ToString();
    }

    public void CollideWithScore(Collider2D col)
    {
        if (chasing)
        {
            return;
        }
        GameObject.Destroy(col.gameObject);
        score += 1;
    }

    public void CollideWithPlayer(PlayerScore otherPlayer)
    {
        if(chasing && !otherPlayer.chasing)
        {
            //we caught them! hooray!
            CatchPlayer();
            otherPlayer.GetCaught();
        }
        else if(!chasing && otherPlayer.chasing)
        {
            GetCaught();
            otherPlayer.CatchPlayer();
        }
    }
}
