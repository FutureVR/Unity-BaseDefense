using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour {

    public GameObject[] bulletTypes;
    public int[] initialBulletAmt;
    public List<int> bulletsLeft;

    Transform bulletSpawn;
    public float forceScale = 30;
    int currBullet = 0;

    void Start ()
    {
        bulletSpawn = GetComponentInChildren<Transform>();

        bulletsLeft = new List<int>();
        for (int i = 0; i < initialBulletAmt.Length; i++)
        {
            bulletsLeft.Add(initialBulletAmt[i]);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown)
        {
            string keyInput = Input.inputString;
            int selection = -1;

            if (keyInput.Length != 0) selection = keyInput[0] - 48;
            if (1 <= selection && selection <= bulletTypes.Length) currBullet = selection - 1;
        }
	}

    public void fire()
    {
        if (bulletsLeft[currBullet] > 0)
        {
            bulletsLeft[currBullet] -= 1;

            Vector3 direction = GetComponentInParent<Camera>().transform.forward;
            GameObject newBullet = Instantiate(bulletTypes[currBullet]) as GameObject;
            newBullet.transform.position = bulletSpawn.position;
            newBullet.GetComponent<Rigidbody>().AddForce(direction * forceScale, ForceMode.Impulse);
        }
    }
}
