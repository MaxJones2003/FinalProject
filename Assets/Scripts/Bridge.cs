using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    SpriteRenderer bridgeSprite;
    PolygonCollider2D block;
    public Sprite brokenBridge;
    public Sprite fixedBridge;
    public bool isFixed;
    AudioSource audioSource;
    public AudioClip fixSound;
    // Start is called before the first frame update
    void Start()
    {
        bridgeSprite = GetComponent<SpriteRenderer>();
        block = GetComponent<PolygonCollider2D>();
        audioSource = GetComponent<AudioSource>();
        bridgeSprite.sprite = brokenBridge;
        isFixed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FixBridge()
    {
        RubyController.instance.WoodAmount(-10);
        isFixed = true;
        audioSource.PlayOneShot(fixSound);
        bridgeSprite.sprite = fixedBridge;
        block.GetComponent<PolygonCollider2D>().enabled = false;
    }
}
