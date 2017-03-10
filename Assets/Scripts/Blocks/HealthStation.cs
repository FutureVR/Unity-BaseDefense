using UnityEngine;
using System.Collections;

public class HealthStation : BaseBlock {

    public float healthIncrease;
    public float woodCostToUse;
    public float metalCostToUse;

    void Start ()
    {
        setTag("Station");
	}

    public override void useThisBock(Person person)
    {
        if (person.woodCarrying - metalCostToUse > 0 && person.metalCarrying - metalCostToUse > 0)
        {
            person.woodCarrying -= metalCostToUse;
            person.metalCarrying -= metalCostToUse;
            person.health += healthIncrease;
        }
    }
}
