using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class IKController : MonoBehaviour
{
    [SerializeField] private Transform m_arm;
    [SerializeField] private Transform m_hand;

    [SerializeField] private Transform m_Leg;
    [SerializeField] private Transform m_Foot;

    [SerializeField] private Transform m_Head;


    [SerializeField] private Transform m_bodyTarget;
    [SerializeField] private Transform m_rightFistTarget;
    [SerializeField] private Transform m_leftFistTarget;
    [SerializeField] private Transform m_headTarget;

    [SerializeField] private Transform m_rightFootTarget;
    [SerializeField] private Transform m_leftFootTarget;

    [SerializeField] private FullBodyBipedIK m_ik;
    [SerializeField] private LookAtIK m_iklk;
    [SerializeField] private float health = 100;
    [SerializeField] private DamageTextSpawner damageTextSpawner;
    [SerializeField] private ParticleSystem particles;

    private float m_speed = 0.8f;
    
    //for hitting
    protected Vector3 firstPosRightHand, firstPosLeftHand, firstPosRightFoot, firstPosHead, firstPosLeftFoot;
    
    public Image img;
    private Vector3 m_vel;
    private Animator anim;
    public Transform RightFistTarget { get { return m_rightFistTarget; } set { m_rightFistTarget = value; } }
    public Transform LeftFistTarget { get { return m_leftFistTarget; } set { m_leftFistTarget = value; } }
    public Transform BodyTarget { get { return m_bodyTarget; } set { m_bodyTarget = value; } }
    public Transform HeadTarget { get { return m_headTarget; } set { m_headTarget = value; } }
    public Transform RightFootTarget { get { return m_rightFootTarget; } set { m_rightFootTarget = value; } }
    public Transform LeftFootTarget { get { return m_leftFootTarget; } set { m_leftFootTarget = value; } }
    public float Health { get { return health; } set { health = value; } }
    public FullBodyBipedIK IK { get { return m_ik; } set { m_ik = value; } }

    public int sign = 1;
    //for getting hit
    public bool gettingHit;
    Vector3 startingPositionBody, startingPositionHead;
    
    public bool punching = false;
    public bool m_FistHitting = false, m_LegHitting = false, m_HeadHitting = false;
    public bool isDead;
    
    public Vector3 force;
    float timer = 3;
    public bool clicking = false;
    Vector3 vel, vel1, vel2, vel3;
    Coroutine inst = null;
    TimeManager timeManager;
    public void Start(){
        timeManager = GameObject.Find("Manager").GetComponent<TimeManager>();
        startingPositionBody = m_bodyTarget.position;
        startingPositionHead = m_headTarget.position;

        firstPosRightHand = m_rightFistTarget.position;
        firstPosLeftHand = m_leftFistTarget.position;
        firstPosRightFoot = m_rightFootTarget.position;
        firstPosHead = m_headTarget.position;
        firstPosLeftFoot = m_leftFootTarget.position;
        m_ik = GetComponent<FullBodyBipedIK>();
        m_iklk = GetComponent<LookAtIK>();
        anim = GetComponent<Animator>();
 
    }

    public void HeadHit(float speed) {
        if (!gettingHit) {
            if (inst != null)
                StopCoroutine(inst);
            StartCoroutine(TransformJiggle(m_headTarget, startingPositionHead, -transform.forward + transform.up, 3f * speed, 3.5f * speed));
        }
    }

    public void BodyHit(float speed) {
        if (!gettingHit){
            StartCoroutine(TransformJiggle(m_bodyTarget, startingPositionBody, -transform.forward, 0.2f * speed, 2f * speed));
        }
    }
    public void LegHit(float speed) { 
        if (!gettingHit) {
            StartCoroutine(TransformJiggle(m_leftFootTarget, firstPosLeftFoot, -transform.forward, 0.2f * speed, speed));
        }
    }

    private void LateUpdate()
    {
        if (clicking || m_FistHitting||m_LegHitting||m_HeadHitting)
        {
            if (m_FistHitting) {
                Vector3 offset = Vector3.zero;
                if (m_hand.position.x> RightFistTarget.position.x)
                     offset = (m_hand.position - RightFistTarget.position);
                else
                     offset = (RightFistTarget.position- m_hand.position);

                // m_arm.localPosition += sign * (m_hand.forward + m_arm.parent.forward) * offset.magnitude / 14;
                // m_hand.localPosition += sign * m_arm.forward * offset.magnitude / 14;

                m_hand.localPosition += new Vector3(0, offset.magnitude / 6, 0);
            }else
            if (m_LegHitting)
            {
                Vector3 offset = (m_Foot.position - RightFootTarget.position);
                m_Leg.localPosition += new Vector3(0, offset.magnitude/14, 0);
                m_Foot.localPosition += new Vector3(0, offset.magnitude / 14, 0);
            }
            else
            if (m_HeadHitting)
            {
                Vector3 offset = (m_Head.position - HeadTarget.position);
                m_Head.localPosition += new Vector3(0, offset.magnitude / 30, 0);
            }
        }
        else
        {
            // m_arm.localPosition = new Vector3(0, 0.1396692f, 0);
            // m_hand.localPosition = new Vector3(0.011f, 0.252f, -0.019f);

            // m_arm.localPosition = new Vector3(0.1059284f, -0.005247984f, -0.02232099f);
            // m_hand.localPosition = new Vector3(0.2784152f, -3.307922e-07f, -1.167631e-07f);
        }
    }
    public void FistHit(Vector3 fce, Transform fistTarget, Vector3 initialPos)
    {
        force = fce;
        StartCoroutine(FistHitting(fistTarget, initialPos));
    }
    public void HeadHit(Vector3 fce)
    {
        force = fce;
        inst = StartCoroutine(HeadHitting());
    }
    public void FootHit(Vector3 fce, Transform footTransform, Vector3 initialTransform)
    {
        force = fce;
        StartCoroutine(LegHitting(footTransform,initialTransform));
    }
    public Transform Pick(Vector3 position)
    {
        float distance = 0;
        distance = Vector2.Distance(new Vector2(position.x, position.y), new Vector2(RightFistTarget.position.x, RightFistTarget.position.y));
        Transform retVal = RightFistTarget;
        if (distance > Vector2.Distance(new Vector2(position.x, position.y), new Vector2(LeftFistTarget.position.x, LeftFistTarget.position.y)))
        {
            distance = Vector2.Distance(new Vector2(position.x, position.y), new Vector2(LeftFistTarget.position.x, LeftFistTarget.position.y));
            retVal = LeftFistTarget;
        }
        if (distance > Vector2.Distance(new Vector2(position.x, position.y), new Vector2(RightFootTarget.position.x, RightFootTarget.position.y)))
        {
            distance = Vector2.Distance(new Vector2(position.x, position.y), new Vector2(RightFootTarget.position.x, RightFootTarget.position.y));
            retVal = RightFootTarget;
        }
        if (distance > Vector2.Distance(new Vector2(position.x, position.y), new Vector2(LeftFootTarget.position.x, LeftFootTarget.position.y)))
        {
            distance = Vector2.Distance(new Vector2(position.x, position.y), new Vector2(LeftFootTarget.position.x, LeftFootTarget.position.y));
            retVal = LeftFootTarget;
        }
        if (distance > Vector2.Distance(new Vector2(position.x, position.y), new Vector2(HeadTarget.position.x, HeadTarget.position.y)))
        {
            retVal = HeadTarget;
        }
        if (retVal == RightFootTarget || retVal == LeftFootTarget)
        { 
            m_LegHitting = true;
        }
        else
        if (retVal == HeadTarget) { 
            HeadHittingInit();
            m_HeadHitting = true;
        }
        else
        if (retVal== RightFistTarget) {
            m_FistHitting = true;
            m_ik.solver.rightHandEffector.positionWeight = 1;
            m_ik.solver.rightArmMapping.weight = 1;
        }else if(retVal == LeftFistTarget)
        {
            m_FistHitting = true;
            m_ik.solver.leftHandEffector.positionWeight = 1;
            m_ik.solver.leftArmMapping.weight = 1;
        }
        return retVal;
    }
    IEnumerator FistHitting(Transform fistTarget, Vector3 initialPosition)
    {
        timer = 0;
        punching = true;

        while (timer < 0.3f&&punching)
        {
            m_FistHitting = true;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            fistTarget.position = Vector3.SmoothDamp(fistTarget.position, fistTarget.position + new Vector3(force.x, force.y, 0) * force.magnitude * m_speed*1.5f, ref vel, 0.2f);
        }
        punching = false;
        
        yield return new WaitForEndOfFrame();

        while (Vector3.Distance(initialPosition, fistTarget.position) > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            fistTarget.position = Vector3.SmoothDamp(fistTarget.position, initialPosition, ref vel1, 0.2f);
            m_ik.solver.rightLegChain.bendConstraint.weight = 0;
        }
        m_FistHitting = false;
        fistTarget.position = initialPosition;
    }
    IEnumerator LegHitting(Transform footTransform, Vector3 initialPosition)
    {
        timer = 0;
        punching = true;
        while (timer < 0.3f && punching)
        {
            m_LegHitting = true;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            footTransform.position = Vector3.SmoothDamp(footTransform.position, footTransform.position + new Vector3(force.x, Mathf.Clamp(force.y, -0.1f, 5), 0) * force.magnitude * m_speed*3, ref vel, 0.2f);
        }
        punching = false;
        Vector3 firstPos = initialPosition;
        yield return new WaitForEndOfFrame();
        while (Vector3.Distance(firstPos, footTransform.position) > 0.0001f &&!gettingHit)
        {
            yield return new WaitForFixedUpdate();
            footTransform.position = Vector3.SmoothDamp(footTransform.position, firstPos, ref vel1, 0.1f);
        }
        m_LegHitting = false;
        footTransform.position = firstPos;
    }
    public void HeadHittingInit() {
        m_ik.solver.rightHandEffector.positionWeight = 0;
        m_ik.solver.rightArmMapping.weight = 0;
        m_ik.solver.leftHandEffector.positionWeight = 0;
        m_ik.solver.leftArmMapping.weight = 0;
    }

    public void HeadHittingLast() {
        StartCoroutine(HeadHittingLasting());
    }
    IEnumerator HeadHittingLasting() {
        float cntr = 0;
        while(cntr<1){
            cntr += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            Mathf.Lerp(cntr, 1, 5 * Time.deltaTime);
            m_ik.solver.rightHandEffector.positionWeight = cntr;
            m_ik.solver.rightArmMapping.weight = cntr;
            m_ik.solver.leftHandEffector.positionWeight = cntr;
            m_ik.solver.leftArmMapping.weight = cntr;
            m_ik.solver.IKPositionWeight = cntr;
        }
    }
    IEnumerator HeadHitting()
    {
        timer = 0;
        punching = true;
        while (timer < 0.3f&&punching)
        {
            m_HeadHitting = true;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            HeadTarget.position = Vector3.SmoothDamp(HeadTarget.position, HeadTarget.position + new Vector3(force.x, force.y, 0) * force.magnitude * m_speed*2f, ref vel, 0.2f);
        }
        punching = false;
        Vector3 firstPos = firstPosHead;
        yield return new WaitForEndOfFrame();
        while (Vector3.Distance(firstPos, HeadTarget.position) > 0.01f&&!gettingHit)
        {
            yield return new WaitForFixedUpdate();
            HeadTarget.position = Vector3.SmoothDamp(HeadTarget.position, firstPos, ref vel1, 0.2f);
        }
        m_HeadHitting = false;
        HeadTarget.position = firstPos;
    }

    IEnumerator TransformJiggle(Transform trans, Vector3 startingPos, Vector3 dir, float distance, float damage) {
        gettingHit = true;
        m_FistHitting = false;
        m_HeadHitting = false;
        m_LegHitting = false;
        health -= damage;
        ShowDamageText(damage);
        if (health <= 0) {
            m_ik.solver.IKPositionWeight = 0;
            m_iklk.solver.IKPositionWeight = 0;
            isDead = true;
            timeManager.DoSlowMotion();
            if (damage > 15){
                anim.SetInteger("Die", 1);
            }
            else { 
                anim.SetInteger("Die", 2);
            }
        }
        img.fillAmount = health / 100;
        while (Vector3.Distance(trans.position, startingPos + dir * distance)>0.01f) {
            yield return new WaitForEndOfFrame();
            trans.position = Vector3.SmoothDamp(trans.position, startingPos + dir*distance, ref m_vel, 0.05f);
        }
        yield return new WaitForEndOfFrame();
        while (startingPos != trans.position){
            yield return new WaitForEndOfFrame();
            trans.position = Vector3.SmoothDamp(trans.position, startingPos, ref m_vel, 0.05f);
        }
        yield return new WaitForEndOfFrame();

        gettingHit = false;
    }

    IEnumerator ReturnToPositions()
    {
        Vector3[] inititalPositions = {firstPosRightHand, firstPosLeftHand, firstPosRightFoot, firstPosHead, firstPosLeftFoot };
        Vector3[] currentPositions = { RightFistTarget.position, LeftFistTarget.position, RightFootTarget.position, HeadTarget.position, LeftFootTarget.position };
        
        for (var i = 0; i < inititalPositions.Length; i++)
        {
            while (Vector3.Distance(inititalPositions[i], HeadTarget.position) > 0.01f)
            {

                yield return new WaitForFixedUpdate();
                currentPositions[i] = inititalPositions[i];
            }
        }
    }

    private void ShowDamageText(float damage)
    {
        if(damageTextSpawner == null)
        {
            Debug.Log("Assign your variable");
            return;
        }
    
        damageTextSpawner.SpawnDamageText(damage);
    }

    public void KnowDownEffect(Vector3 spawnPosition)
    {
        Instantiate(particles, spawnPosition, Quaternion.identity);
    }

}
