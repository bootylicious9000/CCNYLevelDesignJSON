using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile_Elikson : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform firePoint;
    public float Speed = 10f;

    public void Fire()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * Speed, ForceMode2D.Impulse);

    }
}
