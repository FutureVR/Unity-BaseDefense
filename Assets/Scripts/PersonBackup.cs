using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonBackup : MonoBehaviour, IDamageable
{

    public float maxBlockDist = 4;

    bool weaponEquipped = true;
    public float woodCarrying = 100;
    public float metalCarrying = 100;
    float health = 10;
    float armor = 0;
    public float techMultiplier = 2;

    List<Gun> guns = new List<Gun>();
    Shoot shootScript;

    void Start()
    {
        shootScript = GetComponentInChildren<Shoot>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) shootScript.fire();
        if (Input.GetButtonDown("Use")) interactWithBlock("Station");
        if (Input.GetButtonDown("Collect")) interactWithBlock("Resource");
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
                    //blockScript.useThisBock(this);
                }
            }
        }
    }
}
