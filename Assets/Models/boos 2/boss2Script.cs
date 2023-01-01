using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.AI;


using Unity.VisualScripting;
using UnityEngine.UI;

public class boss2Script : MonoBehaviour
{

    public Animator animator ;
    public Transform target;

    public Transform fireBall1;
    public Transform fireBall2;
    public Transform fireBall3;
    public Slider Healthbar;
    public Transform fireBallBig;
    public Transform Bubble ; // the origianl fire ball size

    public float rotationSpeed = 0.1f;
    // public float scaleSpeed = 2f; // scaling the ball
    
    private NavMeshAgent navMeshAgent;
    private bool OnlyOnceInitialWaitTime;
    private bool InitialWaitTime;

    private bool canAttack; 
    private bool canAttackHeavy; 
    
    private bool waitBetweenAttacks; 
    private bool ScreamOnce=false; 



    public ParticleSystem  DamageAnimation ;

    public ParticleSystem  portal ;
    

    public Vector3 currentPostion;  // the last postion player was on 
    public float t = 0.0f;
    public float tBig = 0.0f;
    public bool OnTarget =false;
    public bool  FollowingTarget=false; 

    public Rigidbody rigidbody;

    public Vector3 fireBallPostion1 ; // the origianl fire ball size
    public Vector3 fireBallPostion2 ; // the origianl fire ball size
    public Vector3 fireBallPostion3 ; // the origianl fire ball size

    public Vector3 fireBallBigPostion ; // the origianl fire ball size

   


    public Vector3 BossChargingPosition ; // the origianl fire ball size

    public GameObject  ExplodeAnimation ;
    public GameObject  BigExplodeAnimation ;

    private bool RandomLocation; 

    private Vector3 randomPosition1;
    private Vector3 randomPosition2;
    // public GameObject  ExplodeAnimationReference ;

    private float BoosHealth = 200; // starts with 200 initially 
    public bool DieOnce;
    public bool ChargingOnce;

    public bool isPhase1;// if false mean the second phase
    public bool isCharging; // in phase 2 only

    public bool OnlyOnce; // in phase 2 only

    private bool waitingCharingTime;
    private bool waitBetweenPhases;
    public Vector3 fireBallSizeBig ; // the origianl fire ball size

     public Vector3 BubbleSize ; // the origianl fire ball size

    private bool ShieldOn;

    float scaleSpeed = 1.0f;
    float scaleSpeed2 = 2.0f;


    public AudioSource ThemeSound ;
    public AudioSource FireBallSound ;
    public AudioSource DeathSound ;
    public AudioSource ScreamSound ;
    public AudioSource ChargingSound ;
    public AudioSource ExplodeSound ;



    // Start is called before the first frame update
    void Start()
    {
        ThemeSound.Play();


        navMeshAgent = GetComponent<NavMeshAgent>(); 
        animator = GetComponent<Animator>() ;

        canAttack=false;
        canAttackHeavy=false;
        OnlyOnceInitialWaitTime=false;
        InitialWaitTime=false; // for waiting some time until taking any action
        waitBetweenAttacks=true;

        rigidbody = GetComponent<Rigidbody>(); 

        fireBallPostion1= fireBall1.position;
        fireBallPostion2= fireBall2.position;
        fireBallPostion3= fireBall3.position;

        fireBallBigPostion= fireBallBig.position;
        RandomLocation=false;

        isCharging=true;
      

        isPhase1=true;

        waitingCharingTime=true;

        fireBallSizeBig=  fireBallBig.localScale;

        BubbleSize=  Bubble.localScale;

        waitBetweenPhases=false;
        DieOnce=false;
        ChargingOnce=false;
        OnlyOnce=true;

        ShieldOn=false;
        
        // navMeshAgent.SetDestination(target.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = BoosHealth;
        Debug.Log(BoosHealth);

        if(BoosHealth<=150){ // now will enter phase 2 can tp
            isPhase1=false;
            Debug.Log("pahse 2");
        }

        // fireBallBig.localScale += Vector3.one * scaleSpeed * Time.deltaTime; // scale the ball over the time
        // rotateFireBall();
        // Debug.Log(fireBallPostion);
        // navMeshAgent.SetDestination(target.position);

        if(!OnlyOnceInitialWaitTime){
            OnlyOnceInitialWaitTime=true;
            StartCoroutine(InitialWaitTimeFun(4.0f));
        }


    if(BoosHealth>0){ // alive 

         if(isPhase1){
            navMeshAgent.SetDestination(target.position);

                  
         if(canAttack & waitBetweenAttacks & !OnTarget){ // means in range and waited for 3 sec between attacks
            // animator.StopPlayback();
            FireBallSound.Play();
            fireBall1.gameObject.SetActive(true);
            animator.Play("throw");
            Debug.Log("i am attacking now");
            canAttack=false; // just attacked
            waitBetweenAttacks=false;
            OnTarget=true; 
           
            currentPostion = target.transform.position;
            StartCoroutine("TimeBetweenAttacks");
            }
            else{ // not in range and not waiting the time between attacks
                // animator.StopPlayback();
                // animator.Play("run");
                
            }

                if(OnTarget){
                    Debug.Log("following the player");
                    rigidbody.AddForce(Vector3.up * 9.8f, ForceMode.Acceleration);

                    // Interpolate the object's position using the object's velocity
                    fireBall1.transform.position = Vector3.Lerp(transform.position + new Vector3(0, 20, 0), currentPostion, t);

                    if(!RandomLocation){
                        RandomLocation=true;
                        Vector3 randomPoint1 = Random.insideUnitSphere * 30f;
                        NavMeshHit hit1;
                        NavMesh.SamplePosition(currentPostion + randomPoint1, out hit1, 30f, NavMesh.AllAreas);
                        randomPosition1 = hit1.position;

                        Vector3 randomPoint2 = Random.insideUnitSphere * 30f;
                        NavMeshHit hit2;
                        NavMesh.SamplePosition(currentPostion + randomPoint2, out hit2, 30f, NavMesh.AllAreas);
                        randomPosition2 = hit2.position;
                    }
                    int RandomStart1 = Random.Range(20,50) ; // Attack or TP
                    int RandomStart2 = Random.Range(20,50) ; // Attack or TP
                    fireBall2.transform.position = Vector3.Lerp(transform.position + new Vector3(RandomStart1, 20, 0), randomPosition1, t);
                    fireBall3.transform.position = Vector3.Lerp(transform.position + new Vector3(RandomStart2, 20, 0), randomPosition2, t);

                    // Increase the time
                    t += Time.deltaTime * 1f;
                    Instantiate(ExplodeAnimation, fireBall1.transform.position, transform.rotation);
                    Instantiate(ExplodeAnimation, fireBall2.transform.position, transform.rotation);
                    Instantiate(ExplodeAnimation, fireBall3.transform.position, transform.rotation);
                }

                if (fireBall1.transform.position == currentPostion) { // reset it again
                    RandomLocation=false;
                    Debug.Log("whats it thtaaaaaaaa");
                    fireBall1.gameObject.SetActive(false);
                    fireBall2.gameObject.SetActive(false);
                    fireBall3.gameObject.SetActive(false);
                    OnTarget=false;
                    t = 0.0f;
                    fireBall1.position = fireBallPostion1; // reseting it
                    fireBall2.position = fireBallPostion2; // reseting it
                    fireBall3.position = fireBallPostion3; // reseting it
                    

                
                }

                 if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance  & InitialWaitTime)  { // here will stop going towards the player and start attacking
                        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
                        canAttack= true; 
                        Debug.Log("yes i am in range");
                        // Debug.Log("Agent has reached its stopping distance");
                }
                else{
                    canAttack= false; 
                    Debug.Log("Out of range");
                    animator.StopPlayback();
                    animator.Play("run");
                }

            
        }

        else{ //phase 2
            if(!ScreamOnce){
                ScreamOnce=true;
                ScreamSound.Play();
            }
            

                

        
                if(!waitBetweenPhases & OnlyOnce){ // i want waiting time between the 2 phases
                    OnlyOnce=false;
                    StartCoroutine("TimeBetweenPhases");
                }



                if(waitingCharingTime & canAttackHeavy & waitBetweenPhases){ // i want waiting time at first 
                    Debug.Log("start the attacking");
                    waitingCharingTime=false;
                    StartCoroutine(ChargingTime(5.0f));
                }
            

                if(isCharging && waitBetweenPhases){
                    // Bubble.gameObject.SetActive(false);
                    if(!ChargingOnce){
                        ChargingOnce=true;
                        ChargingSound.Play();
                    }
                   

                    float t = Time.deltaTime * scaleSpeed2;
                    Bubble.transform.localScale = Vector3.Lerp(Bubble.transform.localScale, Vector3.zero, t);

                    ShieldOn=false;
                    Debug.Log("big currenly charging");
                    fireBallBig.gameObject.SetActive(true);
                    // fireBallBig.localScale += Vector3.one * scaleSpeed * Time.deltaTime; // fix this asap
                    currentPostion = target.transform.position;
                    BossChargingPosition=transform.position; // to store the last position the player is on
                    FollowingTarget=true;

                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    string stateName = stateInfo.fullPathHash.ToString();
                    if (stateName != "heavy attack") { // to not force it to play more than once
                        animator.StopPlayback();
                        animator.Play("heavy attack");
                    }
                }
                else if(FollowingTarget){ // on its way to Link
                    ExplodeSound.Play();
                    float t2 = Time.deltaTime * scaleSpeed2;
                    Bubble.transform.localScale = Vector3.Lerp(Bubble.transform.localScale, BubbleSize, t2);
                    // Bubble.localScale = BubbleSize ; // because it will be already charging 
                    // Bubble.gameObject.SetActive(true);
                    ShieldOn=true;
                    Debug.Log("big on the way");
                    fireBallBig.transform.position = Vector3.Lerp(BossChargingPosition + new Vector3(0, 15, 0), currentPostion, tBig);
                    Instantiate(BigExplodeAnimation, fireBallBig.transform.position, transform.rotation);
                    tBig += Time.deltaTime * 1f;
                }


                if (fireBallBig.transform.position == currentPostion) { // reset it again
                        Debug.Log("hit destincation");
                        fireBallBig.gameObject.SetActive(false);
                        tBig = 0.0f;
                        fireBallBig.position = BossChargingPosition + new Vector3(0, 15, 0); // reseting it
                        fireBallBig.localScale = fireBallSizeBig ; // because it will be already charging 
                        FollowingTarget=false;
                        StartCoroutine("TimeBetweenAttacksPhase2");
                        ChargingOnce=false;
                    
                }


                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance & !DieOnce)  { // here will stop going towards the player and start attacking
                        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
                        canAttackHeavy= true; 
                        Debug.Log("yes i am in range");
                            // Debug.Log("Agent has reached its stopping distance");
                }
                else{ // out of range and not charging
                    if(!isCharging){
                        canAttackHeavy= false; 
                        Debug.Log("Out of range");
                        animator.StopPlayback();
                        animator.Play("run");
                    }
                }

                if(!isCharging && !DieOnce){ // follow the layer only if not charging the ball
                    Debug.Log("folowing player");
                    navMeshAgent.SetDestination(target.position);
                }
            


        }



    }

    else{ // dead do whatever you want here (Working fine)
        if(!DieOnce){
            animator.StopPlayback();
            animator.Play("die");
            DeathSound.Play();
            Bubble.gameObject.SetActive(false);
            DieOnce=true;
            if(isCharging){ // died while charging (just remove the ball)
                fireBallBig.gameObject.SetActive(false);
            }
        }

        //could also open a gate or smthing to the next boss
       
    }


   

    
       

    }
        
    

    IEnumerator InitialWaitTimeFun(float time)  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(time);
        InitialWaitTime=true;
      
    }

    IEnumerator ChargingTime(float time)  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(time);
        isCharging=false;
        Debug.Log("done charing");
      
    }


    IEnumerator TimeBetweenAttacksPhase2() { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(5f);
        isCharging=true; 
        waitingCharingTime=true;
        // waitBetweenAttacksPhase2=true;
    }
    


     IEnumerator TimeBetweenAttacks() { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(5f);
       
        waitBetweenAttacks=true;
      
    }

    IEnumerator TimeBetweenPhases() { // if in range will wait this amount of time (to avoid the spamming)
        Debug.Log("in the betwwen pheses");
        animator.StopPlayback();
        animator.Play("scream");
        Bubble.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        waitBetweenPhases=true;
      
    }

    public void DamageBoss(int amount){ // will dmg -40 if hit head
        BoosHealth -=amount ;
    }


    //  public void OnTriggerEnter(Collider c) { // here will break the ball w5las hide + animation
    //     Debug.Log("Asdasd");
    //      if(c.gameObject.CompareTag("Link") & !DieOnce & !ShieldOn){ // not the fucking bubble
    //         string tag = c.gameObject.tag; // just hide it
    //         Debug.Log("Link Hit "+ tag); 
    //         DamageAnimation.Play();
    //         DamageBoss(10);
    //         animator.StopPlayback();
    //         animator.Play("damged");
            
    //         // explosionAnimation.Play();
    //      }

         
    
      
    // }

     void rotateFireBall(){
        fireBallBig.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        fireBallBig.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


     public void TakeDamage(int damage)
    {
        if (!DieOnce && !ShieldOn)
        {
            BoosHealth -= damage;
            animator.StopPlayback();
            animator.Play("damged");
            DamageAnimation.Play();
        }
        if (BoosHealth <= 0)
        {
            Pause.won = true;
        }
    }

    public bool getShieldStatus(){ // the current state 
        return ShieldOn;    
    }
    public bool getPhase(){ // true if phase 1 else 0
        return isPhase1;    
    }


}
