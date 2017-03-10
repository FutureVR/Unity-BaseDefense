using UnityEngine;
using System.Collections;

public class ExplosiveBullet : BaseBullet, IDamageable
{
    public float explosionDamageDealt = 5;
    public float explosionRadius = 15;

    public override void dealDamage(GameObject other)
    {
        dealDamageToObject(other, singleDamageDealt);

        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);

        foreach (Collider c in colliders)
        {
            dealDamageToObject(c.gameObject, explosionDamageDealt);
        }

        die();
    }
}

