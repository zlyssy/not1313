using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health = 100;
    public float speed = 10;
    public float getHealth()
    {
        return health;
    }
    public void setHealth(float helth)
    {
        health = helth;
        if(health <=0)
        {
            Destroy(this.gameObject);
        }
    }
    public float getSpeed()
    {
        return speed;
    }
    public void setSpeed(float sped)
    {
        speed = sped;
    }

}
