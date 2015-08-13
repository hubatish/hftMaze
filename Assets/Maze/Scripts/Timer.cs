using UnityEngine;
using System;
using System.Collections;

namespace ZachUtility
{
    /// <summary>
    /// A helpful class to call an Action and return true after an amount of time has passed
    ///     Advantage over Coroutine - can pause (affected by Update) & restart easily.
    ///     Disadvantage - more code to set up, less efficient
    /// 
    /// Example of use in a MonoBehavior:
    /// Timer timer;
    /// void Start()
    /// {
    ///     //Initialize the timer with 10 steps to go
    ///     timer = new Timer(10);
    ///     timer.Done = delegate
    ///     {
    ///         //Do stuff when finished
    ///         //Perhaps restart the timer?
    ///         timer.Restart();
    ///     }
    /// }
    /// void Update()
    /// {
    ///     timer.Update();
    /// }
    /// </summary>
    public class Timer
    {
        public bool on = false;
        public float maxSteps;
        private float stepsLeft = 0;

        public Action Started;
        public Action Restarted;
        public Action Done;

        public Timer()
        {
            stepsLeft = 0;
            on = false;
        }

        //instantiate Timer and start it with num steps
        public Timer(float steps)
        {
            //Debug.Log("Timer Started");
            maxSteps = steps;
            stepsLeft = maxSteps;
            on = true;
        }

        public void Start(float steps)
        {
            //	Debug.Log("Timer Started");
            maxSteps = steps;
            stepsLeft = maxSteps;
            on = true;

            if (Started != null)
                Started();
        }

        public void ReStart()
        {
            //Debug.Log("Timer ReStarted");
            on = true;
            stepsLeft = maxSteps;

            if (Restarted != null)
                Restarted();
        }

        public bool Update()
        {
            return Update(Time.deltaTime);
        }

        public bool Update(float deltaTime)
        {
            if (on == true)
            {
                stepsLeft -= deltaTime;
                if (stepsLeft <= 0)
                {
                    //Debug.Log("Timer Done");
                    on = false;
                    if (Done != null)
                    {
                        Done();
                    }
                    return true;
                }
            }
            return false;
        }
    }

}

