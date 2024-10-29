using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeScript : MonoBehaviour
{
    public ParticleSystem PS;
    public int ParticleAmt = 10;
    public AudioSource AS;
    public AudioClip Clip;
    public float Lifetime = 0;
    
    void Start()
    {
        if (PS != null)
        {
            PS.Emit(ParticleAmt);
            Lifetime = Mathf.Max(PS.main.duration, Lifetime);
        }

        if (AS != null && Clip != null)
        {
            AS.PlayOneShot(Clip);
            Lifetime = Mathf.Max(Lifetime, Clip.length);
        }
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
