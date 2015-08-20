using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class PlayerManager
{
    private static int _numberPlayers = 0;
    public static int NumberPlayers
    {
        get
        {
            return _numberPlayers;
        }
    }

    private static int numHiders = 0;
    public static int NumberHiding
    {
        get
        {
            return numHiders;
        }
        set
        {
            numHiders = value;
        }
    }

    public static int NumberCaught = 0;

    private static int numSeekers = 0;
    public static int NumberSeeking
    {
        get
        {
            return numSeekers;
        }
    }

    //Is called when a player is added, with the new player passed to listener
    public static Action<MazePlayerUI> playerAddEvent = delegate (MazePlayerUI p) { };

    //Is called when a player is removed, with the removed player passed to listener before removal
    public static Action<MazePlayerUI> playerRemoveEvent = delegate (MazePlayerUI p) { };

    private static IList<MazePlayerUI> _players = new List<MazePlayerUI>();

    /// <summary>
    /// Adds a player to the global list.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static void AddPlayer(MazePlayerUI player)
    {
        playerAddEvent(player);
        _numberPlayers++;
        _players.Add(player);
    }

    public static void CeaseSeeking(MazePlayerUI player)
    {
        if (player.score.chasing)
        {
            numSeekers--;
        }
        else
        {
            numHiders--;
        }
    }

    /// Returns whether this player should be a hider or a seeker
    /// Based on how many of each there are so far
    public static bool ShouldISeek()
    {
        bool seeking = (numHiders > numSeekers);
        if (seeking)
        {
            numSeekers++;
        }
        else
        {
            numHiders++;
        }
        Debug.Log("num s: " + numSeekers);
        Debug.Log("numh: " + numHiders);
        return seeking;
    }

    public static void RemovePlayer(MazePlayerUI player)
    {
        playerRemoveEvent(player);

        CeaseSeeking(player);

        _players.Remove(player);
        _numberPlayers--;
    }

    public static MazePlayerUI GetPlayer(int playerNumber)
    {
        return _players[playerNumber];
    }
}
