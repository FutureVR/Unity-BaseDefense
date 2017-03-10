using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour, IDamageable {

    public float maxBlockDist = 4;

    bool weaponEquipped = true;
    public float woodCarrying = 100;
    public float metalCarrying = 100;
    public float health = 10;
    float armor = 0;
    public float techMultiplier = 2;

    public List<GameObject> gunPrefabs = new List<GameObject>(); //Only for test purposes, delete as soon as possible
    public List<GameObject> guns = new List<GameObject>();
    int currGun = 0;
    public Vector3 gunOffset;
    //Shoot shootScript;

	void Start ()
    {
        addGun(gunPrefabs[0]);
        //addGun(gunPrefabs[1]);
        //shootScript = GetComponentInChildren<Shoot>();
	}
	
	void Update ()
    {
        Debug.Log(guns.Count);

        if (Input.anyKeyDown)
        {
            string keyInput = Input.inputString;
            int selection = -1;

            if (keyInput.Length != 0) selection = keyInput[0] - 48;
            if (1 <= selection && selection <= guns.Count && selection - 1 != currGun)
            {
                switchGun(selection - 1);
                //currGun = selection - 1;
            }
        }

        if (Input.GetButtonDown("Fire1")  &&  guns.Count != 0) guns[currGun].GetComponent<Gun>().fire();
        if (Input.GetButtonDown("Use")) interactWithBlock("Station");
        if (Input.GetButtonDown("Collect")) interactWithBlock("Resource");
	}

    void addGun(GameObject newGun)
    {
        Transform spawnPos = gameObject.transform.GetChild(0).GetChild(0);
        GameObject tempGun = GameObject.Instantiate(newGun, spawnPos) as GameObject;
        tempGun.transform.localPosition = new Vector3(0, 0, 0);
        guns.Add(tempGun);
    }

    void switchGun(int choice)
    {
        guns[currGun].SetActive(false);
        guns[choice].SetActive(true);
        currGun = choice;
    }

    void equipWeapon(bool b)
    {
        
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void die()
    {
        Debug.Log("Player has died");
    }

    void interactWithBlock(string blockTagName)
    {
        Vector3 fwd = gameObject.GetComponentInChildren<Camera>().transform.forward;
        Vector3 pos = gameObject.transform.position;
        RaycastHit hitInfo;
        float maxDist = maxBlockDist;

        if (Physics.Raycast(pos, fwd, out hitInfo, maxDist))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (hitObject.tag == blockTagName)
            {
                //Debug.Log("Hit a Block!");
                BaseBlock blockScript = hitObject.GetComponent<BaseBlock>();
                if (blockScript != null)
                {
                    blockScript.useThisBock(this);
                }
            }
        }
    }
}
