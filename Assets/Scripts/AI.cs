using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : IKController
{
    public Vector3 fistHit;
    float  damp = 0.1f;
    private Vector3 velocity, hitDistance = Vector3.zero;
    private Vector3[] HitPartPositions = new Vector3[3];
    public static float range = 2;
    public static float hitForce = -1.5f;
    IEnumerator Start(){
        base.Start();
        AssignHPbar();
        hitForce -= 0.2f;
        range -= 0.2f;
        if (range < 1)
            range = 1;
        HitPartPositions[0] = RightFistTarget.position;
        HitPartPositions[1] = RightFootTarget.position;
        while (Health > 0) {
            yield return new WaitForSeconds(Random.Range(range, range+1));
            clicking = true;
            Transform tempTrans = Pick(HitPartPositions[Random.Range(0,2)]);
            hitDistance = tempTrans.position + fistHit;
            while (Vector3.Distance(tempTrans.position, hitDistance) >0.01f)
            {
                yield return new WaitForFixedUpdate();
                tempTrans.position = Vector3.SmoothDamp(tempTrans.position, hitDistance, ref velocity, damp);
            }
            yield return new WaitForEndOfFrame();
            clicking = false;
            if (tempTrans == RightFootTarget)
            {
                FootHit(new Vector3(hitForce, 0, 0), RightFootTarget, firstPosRightFoot);
            }
            else
              if (tempTrans == RightFistTarget)
            {
                FistHit(new Vector3(hitForce, 0,0), RightFistTarget, firstPosRightHand);
            }
            else
              if (tempTrans == HeadTarget)
            {
                HeadHit(new Vector3(hitForce*2, 0, 0));
            }
        }
    }

    private void AssignHPbar()
    {
        img = GameObject.Find("Fill AI").GetComponent<Image>();
        img.fillAmount = 1;
    }
}
