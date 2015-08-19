using System;
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

    //Is called when a player is added, with the new player passed to listener
    public static Action<MazePlayerUI> playerAddEvent = delegate (MazePlayerUI p) { };

    //Is called when a player is removed, with the removed player passed to listener before removal
    public static Action<MazePlayerUI> playerRemoveEvent = delegate (MazePlayerUI p) { };

    private static IList<MazePlayerUI> _players = new List<MazePlayerUI>();
    public static int AddPlayer(MazePlayerUI player)
    {
        playerAddEvent(player);
        _numberPlayers++;
        _players.Add(player);
        return _numberPlayers;
    }

    public static int RemovePlayer(MazePlayerUI player)
    {
        playerRemoveEvent(player);
        _players.Remove(player);
        _numberPlayers--;
        return _numberPlayers;
    }

    public static MazePlayerUI GetPlayer(int playerNumber)
    {
        return _players[playerNumber];
    }
}
