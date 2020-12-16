using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

// This script keeps track how many toilet paper the player has and displays it on the UI.
public class PaperCount : MonoBehaviour
{
    public static int count;
    public static int collectedInTotal = 0;

    private Text paperCount;

    void Start()
    {
        paperCount = GetComponent<Text>();

        // Giving the player a starting amount of toilet paper.
        count = 50;
    }


    void Update()
    {
        // Updating the paper counter on the UI.
        paperCount.text = count.ToString();
    }
}
