using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class boss2Script : MonoBehaviour
{

    public Animator animator ;
    public Transform target;
    public Transform fireBall;
    private NavMeshAgent navMeshAgent;
    private bool OnlyOnceInitialWaitTime;
    private bool InitialWaitTime;

    private bool canAttack; 
    private bool waitBetweenAttacks; 

    private float BoosHealth = 50; // starts with 200 initially 


    public ParticleSystem  DamageAnimation ;

    public Vector3 currentPostion;  // the last postion player was on 
    public float t = 0.0f;
    public bool OnTarget =false;

    public Rigidbody rigidbody;

    public Vector3 fireBallPostion ; // the origianl fire ball size

    // Start is called before the first frame update
    void Start()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        animator = GetComponent<Animator>() ;

        canAttack=false;
        OnlyOnceInitialWaitTime=false;
        InitialWaitTime=false; // for waiting some time until taking any action
        waitBetweenAttacks=true;

        rigidbody = GetComponent<Rigidbody>(); 

        fireBallPostion= fireBall.position;
        // navMeshAgent.SetDestination(target.position);
        
    }

    // Update is called once per frame
    void Update()
    {

        if(!OnlyOnceInitialWaitTime){
            OnlyOnceInitialWaitTime=true;
            StartCoroutine(InitialWaitTimeFun(4.0f));

           
        }

        if(OnTarget){
            rigidbody.AddForce(Vector3.up * 9.8f, ForceMode.Acceleration);

            // Interpolate the object's position using the object's velocity
            fireBall.transform.position = Vector3.Lerp(transform.position + new Vector3(0, 20, 0), currentPostion, t);

            // Increase the time
            t += Time.deltaTime * 1f;
        }

         if (fireBall.transform.position == currentPostion) { // reset it again
            fireBall.gameObject.SetActive(false);
            fireBall.position = fireBallPostion;
        }


       


       


    
 

        
        navMeshAgent.SetDestination(target.position);
      

        if(canAttack & waitBetweenAttacks){ // means in range and waited for 3 sec between attacks
            // animator.StopPlayback();
            animator.Play("throw");
            Debug.Log("i am attacking now");
            canAttack=false; // just attacked
            waitBetweenAttacks=false;
            OnTarget=true; 
            fireBall.gameObject.SetActive(true);
            currentPostion = target.transform.position;
            StartCoroutine("TimeBetweenAttacks");
        }
        else{ // not in range and not waiting the time between attacks
            // animator.StopPlayback();
            // animator.Play("run");
            
        }
       


        // Debug.Log(navMeshAgent.remainingDistance);
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
        
    

    IEnumerator InitialWaitTimeFun(float time)  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(time);
        InitialWaitTime=true;
      
    }


     IEnumerator TimeBetweenAttacks() { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(5f);
        waitBetweenAttacks=true;
      
    }

    public void DamageBoss(int amount){ // will dmg -40 if hit head
        BoosHealth -=amount ;
    }


     public void OnTriggerEnter(Collider c) { // here will break the ball w5las hide + animation
        Debug.Log("Asdasd");
         if(c.gameObject.CompareTag("Link")){ // not the fucking bubble
            string tag = c.gameObject.tag; // just hide it
            Debug.Log("Link Hit "+ tag); 
            DamageAnimation.Play();
            DamageBoss(10);
            animator.StopPlayback();
            animator.Play("damged");
            
            // explosionAnimation.Play();
         }

         
    
      
    }
}
