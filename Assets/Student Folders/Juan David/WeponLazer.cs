using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeponLazer : ActorController
{
    public GameObject ProjectilePrefab;
    public GameObject FastProjectilePrefab;
    public GameObject SlowProjectilePrefab;
    public GameObject SpikyProjectilePrefab;
    public Transform firePoint;
    public float Speed = 10f;

    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "ShootProjectile")
        {
            StartCoroutine(ShootProjectile());
        }
        if (act == "ShootFastProjectile")
        {
            StartCoroutine(ShootFastProjectile());
        }
        if (act == "ShootSlowProjectile")
        {
            StartCoroutine(ShootSlowProjectile());
        }
        if (act == "ShootSpikyProjectile")
        {
            StartCoroutine(ShootSpikyProjectile());
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
    public IEnumerator ShootFastProjectile()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(FastProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Speed, ForceMode2D.Impulse);

        yield return null;

    }
    public IEnumerator ShootSlowProjectile()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(SlowProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Speed, ForceMode2D.Impulse);

        yield return null;

    }
    public IEnumerator ShootSpikyProjectile()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(SpikyProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Speed, ForceMode2D.Impulse);

        yield return null;

    }
}

