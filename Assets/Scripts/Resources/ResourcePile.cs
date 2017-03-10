using UnityEngine;
using System.Collections;

public class ResourcePile : BaseBlock
{
    public float collectionRate = 10;
    public float resourceAmt = 50;
    public float damageScalar = 2;

    float baseHeight = .0f;
    float heightFactor = .2f;

    void Start()
    {
        setTag("Resource");
        setHeight();
    }

    public void Update()  
    {

    }

    public void setHeight()
    {
        float size_x = gameObject.transform.localScale.x;
        float size_y = heightFactor * resourceAmt + baseHeight;
        float size_z = gameObject.transform.localScale.z;
        gameObject.transform.localScale = new Vector3(size_x, size_y, size_z);
    }

    //Modifies the resource amount of this pile modifies the variable passed by reference
    public void exchangeResources(ref float personResource, float multiplier)
    {
        loseResources(collectionRate * multiplier);
        personResource += collectionRate * multiplier;

        //if (resourceAmt < 0) die();
    }

    public void loseResources(float amt)
    {
        resourceAmt -= amt;
        if (resourceAmt > 0) setHeight();
        else die();
    }

    public override void takeDamage(float damage)
    {
        loseResources(damage * damageScalar);
    }
}
