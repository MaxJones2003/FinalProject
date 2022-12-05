using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        
            if (controller != null)
            {
                controller.WoodAmount(1);
                Debug.Log(controller.wood);
                Destroy(gameObject);
            }
        

    }
}
