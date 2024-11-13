using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Elikson : ActorController
{
    public GameObject ProjectilePrefab;
    public Transform firePoint;
    public float Speed = 10f;

    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "ShootProjectile")
        {
            StartCoroutine(ShootProjectile());
        }
    }

    public IEnumerator ShootProjectile()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Speed, ForceMode2D.Impulse);

        yield return null;
        
    }
    /*public void Fire()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Speed, ForceMode2D.Impulse);

    }*/
}
