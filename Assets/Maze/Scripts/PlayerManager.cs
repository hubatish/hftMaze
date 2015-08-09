﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    private static IList<MazePlayerUI> _players = new List<MazePlayerUI>();
    public static int AddPlayer(MazePlayerUI player)
    {
        _numberPlayers++;
        _players.Add(player);
        return _numberPlayers;
    }

    public static MazePlayerUI GetPlayer(int playerNumber)
    {
        return _players[playerNumber];
    }
}
