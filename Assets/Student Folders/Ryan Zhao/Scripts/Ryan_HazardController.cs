using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a type of Actor that hurts what it hits
//Useful for enemies, hazards, and projectiles
public class Ryan_HazardController : Ryan_ActorController
{
    //How much damage I deal on collision
    public float Damage = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Did what I hit have an actor script on it?
        Ryan_ActorController act = other.gameObject.GetComponentInParent<Ryan_ActorController>();
        //If so. . .
        if (act != null)
        {
            //Do your effect
            OnHit(act);
        }
        if(other.gameObject.CompareTag("Wall"))
            HitWall(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Did what I hit have an actor script on it?
        Ryan_ActorController act = other.gameObject.GetComponentInParent<Ryan_ActorController>();
        //If so. . .
        if (act != null)
        {
            //Do your effect
            OnHit(act);
        }
        if(other.gameObject.CompareTag("Wall"))
            HitWall(other.gameObject);
    }

    //Gets called when it hits an outer wall
    public virtual void HitWall(GameObject obj)
    {
        //Most things don't care
    }

    //Gets called when it hits another actor
    public virtual void OnHit(Ryan_ActorController act)
    {
        //If you do damage, do damage!
        if (Damage > 0)
        {
            act.TakeDamage(Damage);
        }
    }
}
