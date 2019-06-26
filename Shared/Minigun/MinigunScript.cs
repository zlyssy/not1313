using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunScript : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform laserSpawn;
    public int laserSpeed = 10;
    public int lifetime = 20;
    public float fireRatePerSecond = 10f;
    public float fireAngleDegree = 5;
    public float spinUpTime = 2;
    private float timeHolder = 0;
    private float barrelRotationPerSecond = 72;
    private float spinupTimer;
    public int maxHeat = 100;
    public int cooldownTime = 20;
    private float heat = 0;
    private Animator animating;
    private float cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        animating = transform.GetChild(0).GetComponent<Animator>();
        barrelRotationPerSecond = barrelRotationPerSecond * fireRatePerSecond;
        fireRatePerSecond = 1 / fireRatePerSecond;
        fireAngleDegree = fireAngleDegree / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            animating.SetTrigger("Firing");
            animating.ResetTrigger("NotFiring");
            timeHolder += Time.deltaTime;
            if (spinupTimer < spinUpTime) //spin up animation
            {
                spinupTimer = Mathf.Clamp(spinupTimer + Time.deltaTime, 0, spinUpTime);
                timeHolder = 0;
            }

            //while firing
            if (timeHolder > fireRatePerSecond && spinupTimer >= spinUpTime && heat < maxHeat && cooldown <= 0)
            {
                Fire();
                timeHolder = timeHolder - fireRatePerSecond;
                heat += .1f;
            }

        } 
        else if (spinupTimer > 0) //spin down animation
        {
            animating.ResetTrigger("Firing");
            animating.SetTrigger("NotFiring");
            spinupTimer = Mathf.Clamp(spinupTimer - Time.deltaTime, 0, spinUpTime);
            
        }
        else if(heat > 0)
        {
            heat -= .07f;
        }
        if(heat >= maxHeat)
        {
            cooldown = cooldownTime;
        }
        if (heat > 0)
        {
            heat -= .01f;
        }
        if(cooldown >= 0)
        {
            cooldown -= Time.deltaTime;
        }
        
    }

    
    //firing individual laser bolt
    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab);

        Physics.IgnoreCollision(laser.GetComponent<Collider>(), laserSpawn.parent.GetChild(0).GetChild(0).GetComponent<Collider>());
        laser.transform.position = laserSpawn.transform.position;

        Vector3 rotation = new Vector3(transform.eulerAngles.x + Random.Range(-fireAngleDegree, fireAngleDegree),
            transform.eulerAngles.y + Random.Range(-fireAngleDegree, fireAngleDegree),
            transform.eulerAngles.z);
        laser.transform.rotation = Quaternion.Euler(rotation);

        laser.GetComponent<Rigidbody>().AddForce(laser.transform.forward * laserSpeed, ForceMode.Impulse);
        rotation -= laserSpawn.parent.eulerAngles;
        laser.transform.localRotation = laser.transform.localRotation * Quaternion.Euler(90, 0, 0);

        StartCoroutine(DestroyAfterTime(laser, lifetime));
    }

    //destroying the lasers
    private IEnumerator DestroyAfterTime(GameObject laser, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(laser);
    }
}
