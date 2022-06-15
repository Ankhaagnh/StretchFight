using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public bool head, body, leg;
    private IKController controller;

    private void Start()
    {
        controller = GetComponentInParent<IKController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<IKController>()) {
            if (other.gameObject.GetComponentInParent<IKController>().gameObject == transform.root.gameObject)
                return;
        }

        if (gameObject.GetComponentInParent<IKController>().m_HeadHitting && head)
            return;
        if (!other.GetComponentInParent<IKController>().punching)
            return;
        if (other.tag == "Fist") {
            Vector3 dir = Vector3.one;
            bool hitting = false;

            if (other.gameObject.GetComponentInParent<IKController>()) { 
                dir = other.gameObject.GetComponentInParent<IKController>().force;
                gameObject.GetComponentInParent<IKController>().punching = false;
                hitting = true;
            }
            if (hitting) {
                if (head)
                {
                    controller.HeadHit(dir.magnitude * 1.5f);
                    TriggerEffect();
                }
                if (body)
                {
                    controller.BodyHit(dir.magnitude * 1.5f);
                    TriggerEffect();
                }
                if(leg)
                    controller.LegHit(dir.magnitude * 1.5f);
                TriggerEffect();
            }
        }
        if (other.tag == "Head")
        {
            Vector3 dir = Vector3.one;
            bool hitting = false;
            if (other.gameObject.GetComponentInParent<IKController>())
            {
                dir = other.gameObject.GetComponentInParent<IKController>().force;
                hitting = true;
                gameObject.GetComponentInParent<IKController>().punching = false;

            }
            if (hitting)
            {
                controller.HeadHit(dir.magnitude * 2);
                TriggerEffect();
            }
        }
    }

    private void TriggerEffect()
    {
        if (controller.isDead)
        {
            controller.KnowDownEffect(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1, gameObject.transform.position.z-0.5f));
        }
    }
}
