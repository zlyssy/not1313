using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunWeapon : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        barrelRotationPerSecond = barrelRotationPerSecond * fireRatePerSecond;
        fireRatePerSecond = 1 / fireRatePerSecond;
        fireAngleDegree = fireAngleDegree / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timeHolder += Time.deltaTime;
            if (spinupTimer < spinUpTime)
            {
                spinupTimer = Mathf.Clamp(spinupTimer + Time.deltaTime, 0, spinUpTime);
            }
            
                RotateBarrel(spinupTimer / spinUpTime *
                    barrelRotationPerSecond * Time.deltaTime);
                if (timeHolder > fireRatePerSecond&& spinupTimer >= spinUpTime)
                {
                    Fire();
                    timeHolder = timeHolder - fireRatePerSecond;
                }

        }
        else if (spinupTimer > 0)
        {
            spinupTimer = Mathf.Clamp(spinupTimer - Time.deltaTime, 0, spinUpTime);
            RotateBarrel(spinupTimer / spinUpTime *
                    barrelRotationPerSecond * Time.deltaTime);
        }
    }

    private void RotateBarrel(float angle)
    {
        // laserSpawn.parent.transform.localRotation = laserSpawn.parent.transform.localRotation * Quaternion.Euler(0, 0,
        //  barrelRotationPerSecond * Time.deltaTime);
        laserSpawn.parent.GetChild(0).transform.Rotate(Vector3.forward,angle);
        //transform.GetChild(0).Rotate(0, 0, barrelRotationPerSecond * Time.deltaTime);
    }

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
        laser.transform.localRotation = laser.transform.localRotation * Quaternion.Euler(90,0,0);
        
        StartCoroutine(DestroyAfterTime(laser, lifetime));
    }

    private IEnumerator DestroyAfterTime(GameObject laser, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(laser);
    }
}
