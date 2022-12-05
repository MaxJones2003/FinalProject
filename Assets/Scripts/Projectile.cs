using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject soundPrefab;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 Direction, float force)
    {
        rb.AddForce(Direction * force);
    }

    
    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        EnemyController e = other.collider.GetComponent<EnemyController>();
        Tree t = other.collider.GetComponent<Tree>();
        GameObject sound = Instantiate(soundPrefab);
        if (e != null)
        {
            Destroy(gameObject);
            e.Fix();
            FixedCounter.instance.FixedUI(); //calls the FixedCounter script to keep track of how many robots have been fixed
        }
        
        else if(t != null)
        {           
            t.Break();
            Destroy(gameObject);
        }

        else Destroy(gameObject);
    }
}
