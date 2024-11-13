using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterMovement : HazardController
{
    public override void DoAction(string act, float amt = 0)
    {
        base.DoAction(act, amt);
        if (act == "RandomPosition")
        {
            StartCoroutine(RandomPosition());
        }
    }
    public IEnumerator RandomPosition()
    {
        //I use this to track movement speed
        float moveSpeed = 5;
        //This works a lot like the FourSquare movement blocks, but it's just one
        Vector3 endPos = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f));
        while (transform.position != endPos)
        {
            //Move a percentage of the way there each frame
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);
            //Because Lerp will never actually reach my target, I need to put a tiny MoveTowards in the segment
            //Otherwise I'll end up 0.000001 units away from my target, forever
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f * Time.deltaTime);
            yield return null;
        }
    }
}
