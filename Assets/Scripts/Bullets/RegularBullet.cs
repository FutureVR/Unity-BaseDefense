using UnityEngine;
using System.Collections;

public class RegularBullet : BaseBullet
{
    public override void dealDamage(GameObject other)
    {
        dealDamageToObject(other, singleDamageDealt);
        //die();
    }
}