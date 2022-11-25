using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public static StageController inst { get; private set; }
    public int currentStage;
    int maxStage = 2;
    public int totalRobots; //amount of robots on the current stage
    public int totalCogPickups; //amount of ammo pickups on the stage
    public bool winState = false;

    public Scene stage0;
    public Scene stage1;



    public void Awake()
    {
        inst = this;
        currentStage = 0;
    }

    public void StageSelect()
    {
        if (currentStage == 1 && RubyController.instance.spokeToJambi)
        {
            Stage1();
        }
    }

    public void StageSettings(string sceneName) //Each stage has a saved value for how many robots are on the map, this allows me to use a different number of robots to every scene while keeping my scoring in order
    {
        if (sceneName == "Scene0")
        {
            totalRobots = 6;
            totalCogPickups = 10;
        }
        else if (sceneName == "Scene1")
        {
            totalRobots = 4;
            totalCogPickups = 3;

            currentStage = 1;
        }
    }


    public void Stage1()
    {
        SceneManager.LoadScene("Scene1");
    }
    //this setup allows me to cleanly add more stages without messy code, I can add another stage and its settings using the StageSettings function
}
