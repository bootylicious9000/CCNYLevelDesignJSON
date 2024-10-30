using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActorController : MonoBehaviour
{
    //My rigidbody
    public Rigidbody2D RB;
    //My main sprite
    public SpriteRenderer Body;
    //My animator
    public Animator Anim;
    //My health. MaxHealth records your health at the start of the game
    public float Health;
    private float MaxHealth;
    
    //Your default projectile. Leave null if this object doesn't shoot
    public ProjectileController DefaultProjectile;
    //Spawns this gnome (particle and audio emitter) when you shoot 
    public GnomeScript ShootGnome;
    //Spawns this gnome (particle and audio emitter) when you die
    public GnomeScript DeathGnome;

    void Awake()
    {
        //We do this because you can't make Awake virtual
        OnAwake();
    }

    void Start()
    {
        //We do this because you can't make Start virtual
        OnStart();
        //The GameManager tracks all the actors that exist in the game
        //Add us to it when the scene begins
        GameManager.Singleton.AddActor(this);
    }

    public virtual void OnAwake()
    {
        //MaxHealth imprints from Health at game's start
        MaxHealth = Health;
    }
    
    public virtual void OnStart()
    {
        
    }
    
    /// Deals damage to an actor, killing them if they hit 0 health
    public virtual void TakeDamage(float amt, ActorController source=null)
    {
        //If we don't have health, we don't take damage
        if (MaxHealth <= 0) return;
        //Lower health by amount and die if it hits 0
        Health -= amt;
        if(Health <= 0)
            Die(source);
    }

    /// Called when an actor is reduced to 0HP
    public virtual void Die(ActorController source=null)
    {
        //Default activity on death is to just get deleted
        //You may want to override this with something fancier
        if(DeathGnome != null)
            Instantiate(DeathGnome, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    //Spawns a bullet and calls setup on it
    //Three versions, so you can decide if you want to provide all info or default on it
    public void Shoot(ProjectileController prefab=null)
    {
        Shoot(prefab,transform.position,transform.rotation.eulerAngles);
    }
    public void Shoot(ProjectileController prefab, Vector3 pos)
    {
        Shoot(prefab,pos,transform.rotation.eulerAngles);
    }
    public void Shoot(ProjectileController prefab,Vector3 pos, Vector3 rot)
    {
        //If I don't list a prefab, use my default projectile
        if (prefab == null) prefab = DefaultProjectile;
        if (prefab == null) return;
        //Spawn a bullet
        ProjectileController p = Instantiate(prefab, pos, Quaternion.Euler(rot));
        //Then call setup on it
        p.Setup(this);
        //If I have a shoot gnome set up, spawn it
        if(ShootGnome != null)
            Instantiate(ShootGnome, pos, Quaternion.Euler(rot));
            
    }

    private void OnDestroy()
    {
        //The GameManager tracks all the actors that exist in the game
        //Remove us from it when we leave the scene
        GameManager.Singleton.RemoveActor(this);
    }

    //This gets called by JSON
    public void TakeEvent(EventJSON e)
    {
        //If it calls for an animation, call it!
        if (Anim != null && !string.IsNullOrEmpty(e.Anim))
        {
            Anim.Play(e.Anim);
        }
        //If it calls for an action, run it!
        if (!string.IsNullOrEmpty(e.Action))
        {
            DoAction(e.Action,e.Amt);
        }
    }
    
    //A virtual function meant to be overridden. Gets called whenever you have an event
    //act is equal to the event's "Action" json value
    //Each script you make should have its own override of this
    public virtual void DoAction(string act, float amt = 0)
    {
        //Reads the 'act' that's provided and runs different code depending on the message
        //This is usually a coroutine
        if (act == "Flash")
        {
            StartCoroutine(Flash(amt));
        }
        else if (act == "Shake")
        {
            StartCoroutine(Shake(amt));
        }
    }

    //Makes the actor flash red
    public IEnumerator Flash(float amt)
    {
        if (amt <= 0) amt = 0.5f;
        float bigTime = amt;
        float smTime = 0.1f;
        Color c = Body.color;
        while (bigTime > 0)
        {
            bigTime -= Time.deltaTime;
            smTime -= Time.deltaTime;
            if (smTime <= 0)
            {
                smTime = 0.1f;
                Body.color = Body.color == c ? Color.red : c;
            }
            yield return null;
        }
        Body.color = c;
    }
    
    //Makes the actor screenshake
    public IEnumerator Shake(float amt)
    {
        float time = amt > 0 ? amt : 0.5f;
        Vector3 startPos = Body.transform.localPosition;
        while (time > 0)
        {
            time -= Time.deltaTime;
            Body.transform.localPosition = startPos + 
                new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            yield return null;
        }
        Body.transform.localPosition = startPos;
    }
}
