using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1;
    public bool vertical;
    public float changeTime = 3.0f;
    public int damage; //instead of making a new HardEnemy script, I added a public damage modifier and copied the robot prefab, then i increased the speed and damage to make a new hardRobot

    bool broken = true;

    float timer;
    int direction = 1;



    Rigidbody2D rb;
    Animator animator;
    public ParticleSystem smokeEffect;
    public ParticleSystem hitEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }


    void Update()
    {
        if (!broken)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        Vector2 position = rb.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }


        rb.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(damage);
        }
    }
    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        ParticleSystem hit = Instantiate(hitEffect, rb.position, Quaternion.identity);
        smokeEffect.Stop();
        animator.SetTrigger("Fixed");
    }


}
