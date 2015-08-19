using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Show one player's color/name/stats as part of horizontal layout group this script is attached to
/// </summary>
public class PlayerDisplay : MonoBehaviour
{
    public Text score;
    public Text name;

    public Image image;

    public void Init(int score, string name, Color color)
    {
        Debug.Log("player's core:" + score);
        this.score.text = score.ToString();
        this.name.text = name;
        this.image.color = color;
    }
}

