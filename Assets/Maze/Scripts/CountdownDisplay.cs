using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CountdownDisplay : MonoBehaviour
{
    public DateTime goalTime = new DateTime(2020, 7, 21, 12, 00, 00);
    private Text countdownText;

    [Tooltip("Display lengthended text? (vs abbreviated)")]
    public bool detailDisplay;

    [SerializeField]
    private int minutes = 2;

    //Do stuff after countdown finished
    public Action doneEvent = delegate () { };
    private bool finished = false; //only call that stuff once

    void Start()
    {
        countdownText = GetComponent<Text>();
        RestartTimer();
    }

    public void RestartTimer()
    {
        finished = false;
        goalTime = DateTime.UtcNow + new TimeSpan(0, minutes, 0);
    }

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.UtcNow;
        TimeSpan timeRemaining;

        if (goalTime > currentTime)
        {
            timeRemaining = goalTime - currentTime;

            if (!detailDisplay)
            {
                //Only display 2 time intervals
                TimeSpan inMinutes = new TimeSpan(1, 0, 0);
                TimeSpan inHours = new TimeSpan(4, 6, 0, 0);
                if (timeRemaining <= inMinutes)
                {
                    //within minutes, display seconds
                    countdownText.text = new DateTime(timeRemaining.Ticks).ToString("mm:ss");
                }
                else if (timeRemaining <= inHours)
                {
                    //within hours, display hours & minutes
                    countdownText.text = new DateTime(timeRemaining.Ticks).ToString("HH:mm");
                }
                else
                {
                    //time too long, display in days rather than hours
                    countdownText.text = timeRemaining.Days.ToString() + " D";
                }
            }
            else
            {
                //Display three time intervals/digits
                TimeSpan inMinutes = new TimeSpan(1, 0, 0);
                if (timeRemaining < inMinutes)
                {
                    countdownText.text = timeRemaining.Minutes.ToString() + "m " + timeRemaining.Seconds.ToString() + "s";
                }
                else
                {
                    countdownText.text = timeRemaining.Days.ToString() + "d " + timeRemaining.Hours.ToString() + "h " + timeRemaining.Minutes.ToString() + "m";
                }
            }
        }
        else
        {
            if(!finished)
            {
                doneEvent();
                finished = true;
            }
        }
    }
}