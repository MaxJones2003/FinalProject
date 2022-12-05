using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    AudioSource ad;
    public AudioClip hit;
    float timer = 1.5f;
    void Start()
    {
        ad = GetComponent<AudioSource>();
        ad.PlayOneShot(hit);
    }

    void Update()
    { 
        timer -= Time.deltaTime;

        if (timer < 0)
        {
                Destroy(gameObject);
        }     
    }
    
}
