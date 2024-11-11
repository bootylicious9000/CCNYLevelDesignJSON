using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Elikson : ActorController
{

    public GameObject ProjectilePrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);

        //the speed the bullet travels
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);


    }
}
