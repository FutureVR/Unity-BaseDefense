using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    public GameObject bulletType;
    public int initialBulletAmt = 10;
    public int bulletsLeft;

    Transform bulletSpawn;
    public float forceScale = 30;

    public float woodCost = 100;
    public float metalCost = 100;

    void Start()
    {
        bulletSpawn = GetComponentInChildren<Transform>();
        bulletsLeft = initialBulletAmt;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fire()
    {
        Debug.Log("Firing");
        if (bulletsLeft > 0)
        {
            bulletsLeft -= 1;

            Vector3 direction = gameObject.transform.parent.parent.GetComponentInChildren<Camera>().transform.forward;
            GameObject newBullet = Instantiate(bulletType) as GameObject;
            newBullet.transform.position = bulletSpawn.position + new Vector3(0, 0, 1);
            newBullet.GetComponent<Rigidbody>().AddForce(direction * forceScale, ForceMode.Impulse);
        }
    }
}
