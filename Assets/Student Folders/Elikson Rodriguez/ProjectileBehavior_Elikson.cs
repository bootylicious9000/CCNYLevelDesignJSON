using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior_Elikson : ProjectileController
{
    public override void OnHit(ActorController act)
    { 
        //if the tag is projectile then don't do anything
        if (act.gameObject.CompareTag("Boss_Elikson")) return;
    }
}
