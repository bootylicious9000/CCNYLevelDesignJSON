using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryan_ProjectileController : Ryan_HazardController
{
    
    //How fast I should fly
    public float Speed = 10;
    
    //Who spawned me
    public Ryan_ActorController Source;

    public virtual void Setup(Ryan_ActorController src)
    {
        //Who spawned you?
        Source = src;
    }
    
    private void Update()
    {
        //Just go flying in the direction I'm facing!
        //It's up to the gun to rotate me so that I'm facing the correct direction
        RB.velocity = transform.right * Speed;
    }

    public override void OnHit(Ryan_ActorController act)
    {
        //If you hit the person who spawned you, don't hurt them
        if (act == Source) return;
        //if the tag is projectile then don't do anything
        if (act.gameObject.CompareTag("Projectile")) return;

        //Do your normal payload delivery
        base.OnHit(act);
        
        //You've delivered your payload, self destruct
        Destroy(gameObject);
    }

    public override void HitWall(GameObject obj)
    {
        base.HitWall(obj);
        Destroy(gameObject);
    }
}
