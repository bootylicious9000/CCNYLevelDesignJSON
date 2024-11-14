using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MikyleHazardScript : HazardController
{
    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "FourSquare")
        {
            //StartCoroutine("Name of Attack");
        }
        if (act == "RandomWalk")
        {
            //StartCoroutine("Name of Attack");
        }
    }
}
