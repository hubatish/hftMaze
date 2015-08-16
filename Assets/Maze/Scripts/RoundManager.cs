using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject endScreen;

    public CountdownDisplay countdownTimer;

    protected void Start()
    {
        countdownTimer.doneEvent += delegate ()
        {
            endScreen.SetActive(true);
            GridManager.Instance.gameObject.SetActive(false);
        };
    }
}

