using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Stats health = other.GetComponent<Stats>();
            health.setHealth(health.getHealth()-1);
            
        }
    }
}
