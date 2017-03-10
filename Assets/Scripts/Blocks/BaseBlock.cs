using UnityEngine;
using System.Collections;

public class BaseBlock : MonoBehaviour, IDamageable {

    public float health = 15;
    public float woodCost = 10;
    public float metalCost = 10;


    public void setTag(string tagName)
    {
        if (gameObject.tag != tagName) gameObject.tag = tagName;
    }

    public virtual void takeDamage(float damage)
    {
        health -= damage;
        if (health < 0) die();
    }

    public void die() { Destroy(gameObject); }

    public virtual void useThisBock(Person person) { }
}
