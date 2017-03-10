using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseBullet : MonoBehaviour, IDamageable
{
    public float singleDamageDealt = 2;

    void OnTriggerEnter(Collider other)
    {
        dealDamage(other.gameObject);
        die();
    }

    public virtual void dealDamage(GameObject other)
    {
 
    }

    public virtual void dealDamageToObject(GameObject item, float localDamage)
    {
        MonoBehaviour[] scripts = item.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour mb in scripts)
        {
            if (mb is IDamageable)
            {
                //other.gameObject.GetComponent<EnemyAI>().takeDamage(damage);
                IDamageable damageable = (IDamageable)mb;
                damageable.takeDamage(localDamage);
            }
        }
    }

    public virtual void takeDamage(float damage)
    {

    }

    public virtual void die()
    {
        Destroy(gameObject);
    }
}
