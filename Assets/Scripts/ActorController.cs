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

    private bool HasIdle = false;
    private string CurrentAnim = "";

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

        if (Anim != null)
        {
            foreach (AnimationClip c in Anim.runtimeAnimatorController.animationClips)
            {
                if (c.name == "Idle")
                    HasIdle = true;
            }
        }
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
    
    //Rotate to look at a target. If turnTime is over 0, it takes you turnTime to do a full turn around
    public float LookAt(ActorController targ,float turnTime=0)
    {
        if (targ == null) return 0;
        return LookAt(targ.transform.position,turnTime);
    }
    public float LookAt(GameObject targ,float turnTime=0)
    {
        if (targ == null) return 0;
        return LookAt(targ.transform.position,turnTime);
    }
    public float LookAt(Vector3 targ,float turnTime=0)
    {
        //Calculate what z-rotation you'd need to face the target. This is just trig
        Vector3 diff = targ - transform.position;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //If you turn slowly, only rotate part of the way there
        float z = turnTime > 0 ? Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, rot_z, (180/turnTime) * Time.deltaTime) : rot_z;
        //Plug the rotation you calculated into your actual transform.rotation
        transform.rotation = Quaternion.Euler(0,0,z);
        //Returns how much rotation you still need to do to look at your target 
        return Mathf.Abs(Mathf.DeltaAngle(z, rot_z));
    }

    public void ShootAtPlayer()
    {
        ShootAtPlayer(DefaultProjectile,transform.position);
    }

    public void ShootAtPlayer(ProjectileController prefab)
    {
        ShootAtPlayer(prefab,transform.position);
    }

    public void ShootAtPlayer(ProjectileController prefab, Vector3 pos)
    {
        ShootAt(PlayerController.Player.transform.position,prefab,pos);
    }

    //Shoot at a target
    public void ShootAt(GameObject targ)
    {
        ShootAt(targ.transform.position,DefaultProjectile,transform.position);
    }
    public void ShootAt(GameObject targ, ProjectileController prefab)
    {
        ShootAt(targ.transform.position,prefab,transform.position);
    }
    public void ShootAt(GameObject targ, ProjectileController prefab, Vector3 pos)
    {
        ShootAt(targ.transform.position,prefab,pos);
    }
    public void ShootAt(Vector3 targ)
    {
        ShootAt(targ,DefaultProjectile,transform.position);
    }
    public void ShootAt(Vector3 targ, ProjectileController prefab)
    {
        ShootAt(targ,prefab,transform.position);
    }
    public void ShootAt(Vector3 targ, ProjectileController prefab, Vector3 pos)
    {
        Vector3 diff = targ - pos;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Shoot(prefab,pos,new Vector3(0,0,rot_z));
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
            if(HasIdle) StartCoroutine(TrackAnim(e.Anim));
        }
        //If it calls for an action, run it!
        if (!string.IsNullOrEmpty(e.Action))
        {
            DoAction(e.Action,e.Amt);
        }
    }

    public IEnumerator TrackAnim(string animName)
    {
        CurrentAnim = animName;
        float timer = 0.1f;
        yield return new WaitForSeconds(0.1f);
        float Duration = 0;
        foreach (AnimationClip c in Anim.runtimeAnimatorController.animationClips)
        {
            if (c.name == animName)
                Duration = c.length / Anim.speed;
        }
            
        if (Duration > 0)
        {
            yield return new WaitForSeconds(Duration-0.1f);
            yield return null;
            if (CurrentAnim == animName)
            {
                Anim.Play("Idle");
                CurrentAnim = "";
            }
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
        else if (act == "Shoot")
        {
            Shoot();
        }
        else if (act == "ShootAtPlayer")
        {
            ShootAtPlayer();
        }
        else if (act == "Rotate")
        {
            transform.rotation = Quaternion.Euler(0,0,amt);
        }
        else if (act == "MoveRight")
        {
            transform.position += new Vector3(amt,0,0);
        }
        else if (act == "MoveUp")
        {
            transform.position += new Vector3(0,amt,0);
        }
        else if (act == "TurnOn")
        {
            gameObject.SetActive(true);
        }
        else if (act == "TurnOff")
        {
            gameObject.SetActive(false);
        }
        else if (act == "SetScale")
        {
            transform.localScale = new Vector3(amt, amt, 1);
        }
        else if (act == "SetAnimSpeed")
        {
            if (Anim != null) Anim.speed = amt;
        }
        else if (act == "SetX")
        {
            Vector3 pos = transform.position;
            pos.x = amt;
            transform.position = pos;
        }
        else if (act == "SetY")
        {
            Vector3 pos = transform.position;
            pos.x = amt;
            transform.position = pos;
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
