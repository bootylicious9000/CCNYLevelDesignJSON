using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileManager_Elikson : ActorController
{
    private ShootProjectile_Elikson shoot;

    //public GameObject[numCoins] coins;
    //public GameObject[numHealth] health;

    /*public void OnEnable()
    {
        for (int i = 0; i < numCoins; i++)
        {
            coins *.SetActive(true); *
                }
        for (int x = 0; x < numHealth; x++)
        {
            health[x].SetActive(true);
        }
    }*/
    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "FourSquare")
        {
            //StartCoroutine(FourSquare());
        }
        if (act == "RandomWalk")
        {
            //StartCoroutine(RandomWalk());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Fire();
        //shoot = gameObject.GetComponentInChildren<mousePresent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Opening()
    {
        yield return null;
    }
    
}
