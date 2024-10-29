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
        OnAwake();
    }

    void Start()
    {
        OnStart();
        GameManager.Singleton.AddActor(this);
    }

    public virtual void OnAwake()
    {
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
        if (prefab == null) prefab = DefaultProjectile;
        if (prefab == null) return;
        ProjectileController p = Instantiate(prefab, pos, Quaternion.Euler(rot));
        p.Setup(this);
        if(ShootGnome != null)
            Instantiate(ShootGnome, pos, Quaternion.Euler(rot));
            
    }

    private void OnDestroy()
    {
        
        GameManager.Singleton.RemoveActor(this);
    }

    public void TakeEvent(EventJSON e)
    {
        if (Anim != null && !string.IsNullOrEmpty(e.Anim))
        {
            Anim.Play(e.Anim);
        }
        if (!string.IsNullOrEmpty(e.Action))
        {
            DoAction(e.Action,e.Amt);
        }
    }
    
    public virtual void DoAction(string act, float amt = 0)
    {
        switch (act)
        {
            case "Flash":
            {
                StartCoroutine(Flash(amt));
                break;
            }
            case "Shake":
            {
                StartCoroutine(Shake(amt));
                break;
            }
        }
    }

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
    
    public IEnumerator Shake(float amt)
    {
        float time = amt > 0 ? amt : 0.5f;
        Vector3 startPos = Body.transform.localPosition;
        Debug.Log("SHAKE: " + amt + " / " + time);
        while (time > 0)
        {
            time -= Time.deltaTime;
            Body.transform.localPosition = startPos + 
                new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            yield return null;
        }
        Debug.Log("SHAKE END: " + amt);
        Body.transform.localPosition = startPos;
    }
}
