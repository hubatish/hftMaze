using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public GameObject endScreen;

    public CountdownDisplay countdownTimer;

    [SerializeField]
    private PlayerGridDisplay gridDisplay;

    public static RoundManager Instance;

    [SerializeField]
    private FishSpawner fishspawner;

    protected void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        gridDisplay.onReadyEvent += RestartGame;
        countdownTimer.doneEvent += delegate() {
            EndGame(false); //hiders win if game goes long
        };
        EndGame();
    }

    public void EndGame(bool seekersWon)
    {
        if (seekersWon)
        {
            gridDisplay.titleText.text = "Seekers Won!";
        }
        else
        {
            gridDisplay.titleText.text = "Hiders Won!";
        }
        EndGame();
    }

    public void EndGame()
    {
        endScreen.SetActive(true);
        GridManager.Instance.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        PlayerManager.NumberCaught = 0;

        countdownTimer.RestartTimer();
        GridManager.Instance.gameObject.SetActive(true);
        endScreen.SetActive(false);
        fishspawner.SpawnLotsFish();
    }
}

