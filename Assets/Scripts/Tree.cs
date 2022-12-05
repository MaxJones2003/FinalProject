using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Rigidbody2D tree;
    public GameObject woodPrefab;
    public int woodDropped = 2;
    
    void Start()
    {
        tree = GetComponent<Rigidbody2D>();
    }
    public void Break()
    {
        
        SpawnWood(woodDropped);
        Destroy(gameObject);
    }
    public void SpawnWood(int amount)
    {
        for (int i = 0; i <= amount-1; i++)
        {
            if (i == 0)
            {
                GameObject woodObject = Instantiate(woodPrefab, tree.position + Vector2.up * 0.7f, Quaternion.identity);
            }
            else if (i == 1)
            {
                GameObject woodObject = Instantiate(woodPrefab, tree.position + Vector2.right * 0.7f, Quaternion.identity);
            }
            else if (i == 2)
            {
                GameObject woodObject = Instantiate(woodPrefab, tree.position + Vector2.down * 0.7f, Quaternion.identity);
            }
            else if (i == 3)
            {
                GameObject woodObject = Instantiate(woodPrefab, tree.position + Vector2.left * 0.7f, Quaternion.identity);
            }
            else
            {
                GameObject woodObject = Instantiate(woodPrefab, tree.position, Quaternion.identity);
            }
            
        }
    }
}
