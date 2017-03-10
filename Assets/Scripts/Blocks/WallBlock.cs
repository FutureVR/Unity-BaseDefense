using UnityEngine;
using System.Collections;

public class WallBlock : BaseBlock {

	void Start ()
    {
        setTag("Wall");
	}

    public override void useThisBock(Person person)
    { 
        //Change the person in different ways depending on the type of block        
    }
}
