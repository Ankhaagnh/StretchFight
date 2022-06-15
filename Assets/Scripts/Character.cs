using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : IKController
{
    Vector3 offset = Vector3.zero;
    private Vector3 firstMousePos, lastMousePos;
    private Transform currentTrans = null;
    //Camera stuffs
    Vector3 velOfCamera;
    [SerializeField] float camereMovementStep = 1f;
    bool zoomingCamera;
    float FoVDampingVel;
    private Vector3 camerInitialPosition;
    float desiredFov = 60;
    void Awake()
    {
        camerInitialPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        Vector3 desiredPosition = Camera.main.transform.position;
        if (Health <= 0)
            GameController.controller.gameOver = true;
        if (Input.GetMouseButtonDown(0)&& m_FistHitting == false && m_LegHitting == false && m_HeadHitting == false)
        {
            Vector3 viewPoint = Input.mousePosition;

            viewPoint.z -= Camera.main.transform.position.z;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(viewPoint);

            firstMousePos = worldPosition;
            clicking = true;
            currentTrans = Pick(firstMousePos);
            offset = currentTrans.position - worldPosition;
        }
        else
        if (Input.GetMouseButton(0) && clicking)
        {
            Vector3 viewPoint = Input.mousePosition;
            viewPoint.z -= Camera.main.transform.position.z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(viewPoint);
            currentTrans.position = new Vector3(offset.x + worldPosition.x, offset.y + worldPosition.y, currentTrans.position.z);
            lastMousePos = worldPosition;
            if (clicking && (Input.mousePosition.x > Screen.width / 1.7f || Input.mousePosition.y > Screen.height / 1.5f))
            {
                if (currentTrans == RightFootTarget)
                {
                    FootHit(firstMousePos - lastMousePos, RightFootTarget, firstPosRightFoot);
                }
                else
                if (currentTrans == LeftFootTarget)
                {
                    FootHit(firstMousePos - lastMousePos, LeftFootTarget, firstPosLeftFoot);
                }
                else
                if (currentTrans == RightFistTarget)
                {
                    FistHit(firstMousePos - lastMousePos, RightFistTarget, firstPosRightHand);
                }
                else
                if (currentTrans == LeftFistTarget)
                {
                    FistHit(firstMousePos - lastMousePos, LeftFistTarget, firstPosLeftHand);
                }
                else
                if (currentTrans == HeadTarget)
                {
                    HeadHit(firstMousePos - lastMousePos);
                }
            }
        }
        else
        if (Input.GetMouseButtonUp(0) && clicking)
        {
            clicking = false;
            Vector3 viewPoint = Input.mousePosition;
            viewPoint.z -= Camera.main.transform.position.z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(viewPoint);

            lastMousePos = worldPosition;

            if (currentTrans == RightFootTarget)
            {
                FootHit(firstMousePos - lastMousePos, RightFootTarget, firstPosRightFoot);
            }
            else
            if (currentTrans == LeftFootTarget)
            {
                FootHit(firstMousePos - lastMousePos, LeftFootTarget, firstPosLeftFoot);
            }
            else
            if (currentTrans == RightFistTarget)
            {
                FistHit(firstMousePos - lastMousePos,RightFistTarget, firstPosRightHand);
            }
            else
            if (currentTrans == LeftFistTarget)
            {
                FistHit(firstMousePos - lastMousePos, LeftFistTarget, firstPosLeftHand);
            }
            else
            if (currentTrans == HeadTarget)
            {
                HeadHit(firstMousePos - lastMousePos);
            }
        }
        float damping = 0.5f;
        if (clicking && (Input.mousePosition.x <= Screen.width * 0.1f)){
            desiredPosition += new Vector3(camereMovementStep, 0, 0);
            desiredPosition = new Vector3(Mathf.Clamp(desiredPosition.x, -5f, -3.71f), desiredPosition.y, desiredPosition.z);
            desiredFov += 1f;
            desiredFov = Mathf.Clamp(desiredFov, 60, 70f);
        } 
        else{
            desiredPosition = camerInitialPosition;
            damping = 0.2f;
            desiredFov = 60;
        }
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, desiredPosition, ref velOfCamera, damping);
        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, desiredFov, ref FoVDampingVel, damping);
    }
}
