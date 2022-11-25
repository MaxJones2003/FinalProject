using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip background;
    public AudioClip win;
    public AudioClip lose;
    int x = 0; //this variable makes sure the win and loss music only play once

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        audioSource.clip = background;

        audioSource.Play();
        Debug.Log(RubyController.instance.winCondition + " win test");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(RubyController.instance.winCondition + "win test");
        //Debug.Log(RubyController.instance.loseCondition + "lose test");

        if (RubyController.instance.winCondition && x == 0)
        {
            x++;
            audioSource.Stop();
            audioSource.clip = win;
            audioSource.Play();
        }
        if (RubyController.instance.loseCondition && x == 0)
        {
            x++;
            audioSource.Stop();
            audioSource.clip = lose;
            audioSource.Play();
        }
    }

}

