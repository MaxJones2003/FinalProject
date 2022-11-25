using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FixedCounter : MonoBehaviour
{
    public static FixedCounter instance { get; private set; }
    public int fixedCount = 0;
    public int totalCount;
    public int remainingRobots;
    TextMeshProUGUI UIText;

    void Start()
    {
        instance = this;
        UIText = GetComponent<TextMeshProUGUI>();
        totalCount = StageController.inst.totalRobots;
        UIText.text = "Fixed Robots: " + fixedCount + "/" + totalCount;
    }

    public void FixedUI()
    {
        fixedCount++;
        UIText.text = "Fixed Robots: " + fixedCount + "/" + totalCount;
    }

    void Update()
    {
        remainingRobots = totalCount - fixedCount; //calculates how many robots are left in the stage (remainingRobots is used in the RubyController for entering the lose state)
        if (remainingRobots == 0) //checks if there are any robots left
        {
            if (StageController.inst.currentStage == 0)
            {
                RubyController.instance.TalkToJambi();
            }
        }
    }
}
