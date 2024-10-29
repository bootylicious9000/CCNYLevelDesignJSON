using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : ActorController
{
    //How much damage I deal on collision
    public float Damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Did what I hit have an actor script on it?
        ActorController act = other.gameObject.GetComponentInParent<ActorController>();
        //If so. . .
        if (act != null)
        {
            //Do your effect
            OnHit(act);
        }
        if(other.gameObject.CompareTag("Wall"))
            HitWall();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Did what I hit have an actor script on it?
        ActorController act = other.gameObject.GetComponentInParent<ActorController>();
        //If so. . .
        if (act != null)
        {
            //Do your effect
            OnHit(act);
        }
        if(other.gameObject.CompareTag("Wall"))
            HitWall();
    }

    public virtual void HitWall()
    {
        //Most things don't care
    }

    public virtual void OnHit(ActorController act)
    {
        Debug.Log("HIT " + act + " / " + Damage);
        //If you do damage, do damage!
        if (Damage > 0)
        {
            act.TakeDamage(Damage);
        }
    }
}
