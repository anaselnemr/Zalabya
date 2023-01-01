using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using System.Threading.Tasks;

public class bossScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator ;
    public Slider Healthbar;
    public Transform target;
    public Transform fireBall;
     public Transform bubble;
    private NavMeshAgent navMeshAgent;
    // public Transform ground;
    // public ParticleSystem explosionAnimation ;
    public ParticleSystem  chargeAnimation ;
    public ParticleSystem  DamageAnimation ;
    public ParticleSystem  BubbleAnimation ;
    public ParticleSystem  Phase2Animation ;
    public ParticleSystem  HitAnimation ;
    // public bool attacking=false ; 
    // public Transform point1;
    // public Transform point2;

    // Vector3 startPos = transform.position;
    // Vector3 endPos = new Vector3(0, 0, 0);
    public float t = 0.0f;
    // public float lerpSpeed = 0.5f;
    public Vector3 currentPostion;  // the last postion player was on 
    public bool OnTarget;

    public bool CharggingBall ;
    
    // for the floating animation
   
    public float amplitude = 1f;
    public float frequency = 1f;

    public float amplitude2 = 0.5f;
    public float frequency2 = 0.5f;

    private float startTime;

    private bool canAttack=false; 

    private bool HasFall =false ;
    private bool WaitingHasFall =false ;
    

    public Rigidbody rigidbody;
    public LayerMask groundLayerMask;
    public float fallSpeed = 9.8f;
    public float groundDistance = 0.2f;

    public float baseOffset;
    public float baseOffsetDelta = 5f;
    public float baseOffsetTemp;

    public float scaleSpeed = 5f; // scaling the ball
    public Vector3 fireBallSize ; // the origianl fire ball size
    public Vector3 fireBallPostion ; // the origianl fire ball size

    public float rotationSpeed = 800.0f;

    private bool isFallOnHead ;
    public float FallingOnHeadSpeed = 1f;
    public bool onlyOnceOnHead ;

    public float fartherThanAgentBy =200f; // the rdanadom place where it will be resawpned 
    public float fartherThanLastLocation =50f; // the rdanadom place where it will be resawpned 

    public bool isPhase1;// if false mean the second phase
    public bool FinishedAllAni = true ; // if false mean the second phase


    public bool WaitingBetweenAttacks;
    public bool WaitingBetweenTp ;
    public bool WaitingBetweenDiffActions ;

    Vector3 newPosition;

    Coroutine ChargingBallAndTpCoroutine;

    private bool LastActionWasFall;
    private bool MustTpNow;

    private float fireBallSpeed;
    

    private float BoosHealth = 200; // starts with 200 initially 

    private bool InitialWaitTime;
    private bool OnlyOnceInitialWaitTime;

    public bool DieOnce;

    public GameObject TpAnimation; // I will generate Some Grounds
    public GameObject TpAnimationReference; // to assign the prefabe to

    private Vector3 lastPosition; // will store the last postion the agent tp from

    private bool EnterOnce;
    

    
    public AudioSource ThemeSound ;
    //  public AudioSource ScreamSound ;
    // public AudioSource FireBallSound ;
    public AudioSource DeathSound ;
   
    public AudioSource FireBallHitSound ;
    public AudioSource ExplodeSound ;
    public GameObject Message;




   
    


    

    void Start()
    {
        // chargeAnimation.Stop();
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        animator = GetComponent<Animator>() ;

        OnTarget =false; // all are false   
        CharggingBall=false;
        fireBall.gameObject.SetActive(false);
      
        // chargeAnimation.loop = true;
        startTime = Time.time;

        rigidbody = GetComponent<Rigidbody>(); 
        baseOffset=navMeshAgent.baseOffset;
        baseOffsetTemp=navMeshAgent.baseOffset;

        fireBallSize=  fireBall.localScale;
        fireBallPostion= fireBall.position;

        isFallOnHead=false;
        onlyOnceOnHead=true;

        isPhase1 = true ;
        WaitingBetweenAttacks=true ;
        WaitingBetweenTp=true ;
        WaitingBetweenDiffActions=true;
        LastActionWasFall=false;
        MustTpNow=false;

        fireBallSpeed=1f;

        InitialWaitTime=false; // for waiting some time until taking any action
        OnlyOnceInitialWaitTime=false;

        DieOnce=false;

        EnterOnce=false;
        
        



        

        // transform.LookAt(fireBall.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = BoosHealth;
    if (BoosHealth >0 ){ // still alive



        
        if(!OnlyOnceInitialWaitTime){
            OnlyOnceInitialWaitTime=true;
            StartCoroutine(InitialWaitTimeFun(7.0f));

        }
        Debug.Log(BoosHealth);

        if(BoosHealth<=100){ // now will enter phase 2 can tp
            isPhase1=false;
        }

    if (!isPhase1) { // can tp in 2 cases (while charging and idle) 

            if(baseOffsetTemp>=baseOffset){
                Phase2Animation.gameObject.SetActive(true);
            }
            
            Debug.Log("the 2nd Phase");
            fireBallSpeed=1.2f;
            
            int ActionType = Random.Range(1,11) ; // Attack or TP
            if(LastActionWasFall){ // the first action will be made after standing up will be always tp behind the enemy
                LastActionWasFall=false;
                WaitingBetweenDiffActions=false; 
                StartCoroutine("timeAfterStandingToTp");
            }
            if(MustTpNow){
                WaitingBetweenDiffActions=true;
                ActionType=1 ; // just enter the tp
            }
            

            if (WaitingBetweenDiffActions){ // 70% will attack
                    switch (ActionType)  {
                        case int n when (n >= 1 && n <= 3 ):{ // the tp in idle 
                            MustTpNow=false; // will tp if you are so close to it
                            if (canAttack && FinishedAllAni ){ 
                                Debug.Log("will tp");
                                WaitingBetweenDiffActions=false; 
                                TpAnimationReference = Instantiate(TpAnimation, transform.position, transform.rotation);
                                StartCoroutine(timeBetweenDiffActions(1.5f));
                                do
                                {
                                    newPosition = RandomNavmeshLocation2(100.0f);
                                } while (Vector3.Distance(newPosition, target.position) < fartherThanAgentBy
                                  );
                                navMeshAgent.Warp(newPosition);
                                lastPosition = newPosition;
                            }
                        }; break;
                        case int n when (n >= 4 && n <= 10 ):{ // normal attack
                               
                            if (canAttack && FinishedAllAni ){ 
                                Debug.Log("will attack");
                                WaitingBetweenDiffActions=false; 
                                StartCoroutine(timeBetweenDiffActions(7.0f)); // taking into considiration the charging animation
                                performTheAttackAction() ;
                            }
                        }; break;
                        
                        default: break;
                    }
            }
        }

        if(isPhase1 & InitialWaitTime){ // can only attack the player
            Debug.Log("the 1st Phase");
            fireBallSpeed=1f;
            if (canAttack && FinishedAllAni ){ // can attack means in range
                if (WaitingBetweenAttacks){
                        Debug.Log("in WaitingBetweenAttacks");
                        WaitingBetweenAttacks=false; 
                        performTheAttackAction() ;
                        StartCoroutine("timeBetweenAttacks");
                }
            }
        }
       
        // Debug.Log( navMeshAgent.remainingDistance);
        
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)  { // here will stop going towards the player and start attacking
           
            
            // Debug.Log( navMeshAgent.stoppingDistance);
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
            canAttack= true; 
            Debug.Log("yes i am in range");
            // Debug.Log("Agent has reached its stopping distance");
        }
        else {
            canAttack= false; 
            Debug.Log("Out of range");
        }

        if(Input.GetKeyDown("]")){ // to prevent more than one jump
            // animator.SetTrigger("isCharging") ;
            animator.Play("fall");
            HasFall=true; // will be grounded and no bubble is around him anymore
            isFallOnHead=true;
            // transfor.postion=transform.Translate(new Vector3(0, 0, 0));
            // animator.gameObject.GetComponent<Animator>().enabled = false;
        }  

        if(Input.GetKeyDown("e")){ //switch between phases
            isPhase1=!isPhase1;
        }  


        // if (canAttack && FinishedAllAni){ // check its range
        //      Debug.Log("i can ttack");   
        //     if(Input.GetKeyDown("space")){ // to prevent more than one jump
        //         // animator.SetTrigger("isCharging") ;
        //         // animator.SetBool("isCharging2", true);
        //         // animator.StopPlayback();
        //         // animator.SetTrigger("isCharging") ;
        //         FinishedAllAni=false ;
        //         animator.Play("charging");
        //         OnTarget=false;
        //         fireBall.gameObject.SetActive(false);
        //         CharggingBall=true ;
        //         fireBall.transform.position = transform.position +new Vector3(0, 8, 0) ;
        //     }  

        // }
       

      
        if(OnTarget){ // on her way to the player
            Debug.Log("on taget");
            chargeAnimation.Stop();
            fireBall.transform.position = Vector3.Lerp(transform.position +new Vector3(0, 8, 0), currentPostion , t);
            t += Time.deltaTime*fireBallSpeed;
            rotateFireBall();
        }
        else{

            // fireBall.localScale += Vector3.one * scaleSpeed * Time.deltaTime; // scale the ball over the time
            rotateFireBall();
         
            if(CharggingBall){ // here either will teleport or not
                EnterOnce=true;
                fireBall.localScale = fireBallSize ; // because it will be already charging 
                Debug.Log("start charging the ball");
                CharggingBall=false ;
                if(!isPhase1){
                    int range = Random.Range(1,11) ; // Attack or TP
                    if(range>=1 && range<=8){ //80% chance will tp while charging in phase 2
                        int timeInCharge = Random.Range(1,4) ; // Attack or TP
                        // TpAnimationReference = Instantiate(TpAnimation, transform.position, transform.rotation);
                        ChargingBallAndTpCoroutine=StartCoroutine(ChargingBallAndTp(timeInCharge));
                    }
                }
                StartCoroutine("ChargeBall");
            }
            
            
        }

        if (fireBall.transform.position == currentPostion) { // reset it again
            // Debug.Log("Lerp has reached the destination");
            // fireBall.gameObject.SetActive(false);
            // FireBallHitSound.Play();
            fireBall.localScale = fireBallSize ;
            FinishedAllAni=true;
          
            // await Task.Delay(2000);
            // explosionAnimation.Play();  // upon the hit the play the animation
          
        }

        if(!HasFall){ // 
            // Debug.Log("back to normal");
            floatAnimation();
            navMeshAgent.SetDestination(target.position);
            bubble.gameObject.SetActive(true);
            

            if (!BubbleAnimation.isPlaying) {
                Debug.Log("am playing now");
                BubbleAnimation.gameObject.SetActive(true);
                BubbleAnimation.Play();
            }
            if(baseOffsetTemp<baseOffset){
                baseOffsetTemp += baseOffsetDelta * Time.deltaTime;
                navMeshAgent.baseOffset = baseOffsetTemp;
            }
            // Debug.Log("in if maan");
        }
        else{ // is fall now cant do shitt or currently attacking
            if (HasFall){
                try{
                    StopCoroutine(ChargingBallAndTpCoroutine);
                }
                catch{
                    Debug.Log("some kind of error");
                }
                
               
                
                // Debug.Log(baseOffset);
                bubble.gameObject.SetActive(false);

                if (BubbleAnimation.isPlaying) {
                    BubbleAnimation.Stop();
                }

                if(onlyOnceOnHead){
                    onlyOnceOnHead=false;
                    StartCoroutine("FallingOnHeadBall");
                }

                if(isFallOnHead){
                    Vector3 newPosition = fireBall.position;
                    newPosition.y -= FallingOnHeadSpeed * Time.deltaTime;
                    fireBall.position = newPosition;
                    // Debug.Log(fireBallPostion);
                    if(newPosition.y<fireBallPostion.y-5){ // has falling on the head 
                        TakeDamage(40); // on his head by 40
                        StopCoroutine("FallingOnHeadBall");
                        isFallOnHead=false ;
                        WaitingHasFall=true ;
                        fireBall.gameObject.SetActive(false);
                        Debug.Log("has reached the goal");
                    }
                    Debug.Log("in isFallOnHead");
                }
               
               

                // fireBall.transform.position.y -= baseOffsetDelta * Time.deltaTime;

                if(baseOffsetTemp>0 && !isFallOnHead){ // this is what makes the boss fall 
                    baseOffsetTemp -= baseOffsetDelta * Time.deltaTime;
                    navMeshAgent.baseOffset = baseOffsetTemp;
                }

                //  Vector3 newPosition = ;
               
                // targetObject.position = newPosition;
                
                if(WaitingHasFall){  // the fucking proooooooooooooooooooooooooooooob
                    // Debug.Log("in the if");
                    Debug.Log("i entered this");
                    StopCoroutine("ChargeBall"); // will stop the throw animation
                    WaitingHasFall=false;
                    StartCoroutine("HasFallTime");
                    // fireBall.gameObject.SetActive(false); // just close it in case was opened
                }



            }
            

            else if (!OnTarget){// nothing


            }
           
         
        }

    }
    else{ // dead do whatever you want here (Working fine)
            Phase2Animation.gameObject.SetActive(false);
            if(!DieOnce){
            animator.StopPlayback();
            animator.Play("death");
            DieOnce=true;
            StopCoroutine("HasFallTime");
                StartCoroutine(Next());
        }

       if(baseOffsetTemp>0){ // this is what makes the boss fall 
                    baseOffsetTemp -= baseOffsetDelta * Time.deltaTime;
                    navMeshAgent.baseOffset = baseOffsetTemp;
        } 
        //could also open a gate or smthing to the next boss
       
    }
       

        
       
        
    }

    IEnumerator Next()
    {
        Message.SetActive(true);
        yield return new WaitForSeconds(3);
       SceneManager.LoadScene(4);
    }
    IEnumerator ChargeBall()  {
        
      
        // float waitTime= 4.0f;
        // if(HasFall){ // i dont want it to wait lol
        //     waitTime=0.0f;
        // }

      
        // if (!HasFall){
            Debug.Log("will hit the enemy");
            yield return new WaitForSeconds(4.0f);
            OnTarget=true;
            t = 0.0f;
            currentPostion = target.transform.position;
            animator.StopPlayback();
            animator.Play("throw");
            // Debug.Log("after 4 second");
        // }
        // else{ // this means just make the ball hit the boss
        //     Debug.Log("hit the head");

        // }
      
        // animator.SetBool("isCharging2", false);
        // animator.SetTrigger("throughBall") ;
        
      
    }
    public void  floatAnimation(){
        float elapsedTime = Time.time - startTime;

        // Calculate the vertical movement using a sine wave
        float verticalMovement = amplitude * Mathf.Sin(frequency * elapsedTime);
        // float horizontalMovement = amplitude * Mathf.Cos(frequency * elapsedTime);

        // Translate the GameObject vertically
        transform.Translate(new Vector3(0, verticalMovement, 0));
        // navMeshAgent.baseOffset = verticalMovement +baseOffsetTemp;
    }

//     public void floatAnimation(){
//     float elapsedTime = Time.time - startTime;

//     // Calculate the vertical movement using a sine wave
//     float verticalMovement = amplitude * Mathf.Sin(frequency * elapsedTime);

//     // Calculate the depth movement using a cosine wave
//     float depthMovement = amplitude2 * Mathf.Cos(frequency2 * elapsedTime);

//     // Translate the GameObject vertically and depth
//     transform.Translate(new Vector3(0, verticalMovement, depthMovement));
// }


     IEnumerator HasFallTime()  {
        // animator.StopPlayback();
        yield return new WaitForSeconds(6.0f);
        // animator.StopPlayback();
        // animator.SetTrigger("isStandUp") ;
        // animator.enabled =true;
        animator.StopPlayback();
        animator.Play("idle");
        // animator.SetTrigger("isStandUp") ;
        Debug.Log("I am triggereddddd");
        HasFall=false;
        WaitingHasFall=false;
        onlyOnceOnHead=true; // reset it
        FinishedAllAni=true;
        StopCoroutine("timeBetweenAttacks"); // will stop the throw animation
        WaitingBetweenAttacks=false; 
        StartCoroutine("timeBetweenAttacksSlow"); // will start another session
        LastActionWasFall=true; 
        // baseOffsetTemp=baseOffset ; // reset it again
        // navMeshAgent.baseOffset = baseOffsetTemp;
        // Debug.Log("justed funsied ");
        // animator.Play("throw");
    }
    

    //  void OnTriggerEnter(Collider c) {
    //     // This function is called when the collider enters the trigger
    //     //  if(c.gameObject.CompareTag("ground")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
    //     //     Destroy(fireBall); 
    //     // }
    //     Debug.Log("Collider entered the trigger");
    // }


    bool IsGrounded(){
        // Cast a ray downward to check if the player is touching the ground
        // Debug.Log("ENTEEEEEED");
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, transform.forward * 1000, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundDistance, groundLayerMask))
        {
            // The player is touching the ground
            return true;
        }
        else
        {
            // The player is not touching the ground
            return false;
        }
    }

    void rotateFireBall(){
        fireBall.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        fireBall.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }


     IEnumerator FallingOnHeadBall()  {
        yield return new WaitForSeconds(1.0f);
       isFallOnHead=false ;
       WaitingHasFall=true ;
    }


      Vector3 RandomNavmeshLocation2(float radius) { // any random place
        // Generate a random point within the given radius
        Vector3 randomPoint = Random.insideUnitSphere * radius;

        // Convert the point to a NavMesh location
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    Vector3 RandomNavmeshLocation1(float radius){ // only behind the player
    // Calculate the direction from the player to the agent
    Vector3 direction = transform.position - target.position;
    direction = Vector3.Normalize(direction);

    // Generate a random point within the given radius
    Vector3 randomPoint = Random.insideUnitSphere * radius;

    // Convert the point to a NavMesh location
    NavMeshHit hit;
    NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);

    // Return the position behind the player
    return target.position + Vector3.Scale(direction, new Vector3(-1, 1, -1)) * radius;
}

    public void performTheAttackAction(){ // this will be called
        FinishedAllAni=false ;
        animator.Play("charging");
        OnTarget=false;
        fireBall.gameObject.SetActive(true);
        CharggingBall=true ;
        fireBall.transform.position = transform.position +new Vector3(0, 8, 0) ;
    }

     IEnumerator timeBetweenAttacks()  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(7.0f);
        WaitingBetweenAttacks=true;
      
    }

    IEnumerator timeBetweenAttacksSlow()  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(3.0f);
        WaitingBetweenAttacks=true;
      
    }


     IEnumerator timeBetweenTps()  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(3.0f);
        WaitingBetweenTp=true;
      
    }

    IEnumerator timeBetweenDiffActions(float time)  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(time);
        try{
            Destroy(TpAnimationReference) ;
        }
        catch{
            Debug.Log("not found yet errrror");
        }
      
        WaitingBetweenDiffActions=true;
      
    }

    IEnumerator ChargingBallAndTp(float time)  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(time);
         
        // try{
        //     Destroy(TpAnimationReference) ;
        // }
        // catch{
        //     Debug.Log("not found yet errrror");
        // }
        TpAnimationReference = Instantiate(TpAnimation, transform.position, transform.rotation);
        Debug.Log("will tp while charging");
        do
        {
            newPosition = RandomNavmeshLocation2(100.0f);
        } while (Vector3.Distance(newPosition, target.position) < fartherThanAgentBy);
        navMeshAgent.Warp(newPosition);
        lastPosition = newPosition;
    }


    IEnumerator timeAfterStandingToTp()  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(1f);
        MustTpNow=true;
      
    }

    public void RockHitBoss(){ // will dmg -40 if hit head
        BoosHealth-=40 ;
    }
/*    public void DamageBoss(int amount){ // will dmg -40 if hit head
        BoosHealth -=amount ;
    }*/


    // void OnCollisionEnter(Collision c){
    //     // Debug.Log("i have colided with something"); 
    //    if(c.gameObject.CompareTag("fireBall")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
    //       MyHealth-=4 ; // fire ball will dmg by 4
    //       Debug.Log("i have been dmg");
    //     }
    //  }
    public void TakeDamage(int damage)
    {
        if (HasFall && !DieOnce)
        {
            Debug.Log("IN FLOOR AND DAMGED");
            BoosHealth -= damage;
            DamageAnimation.Play();
        }
//         if (BoosHealth <= 0)
//         {

// /*            a.PlayInFixedTime("Die");*/
//             StartCoroutine(DestroyEnemy());
//         }

    }
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider c) { // here will break the ball w5las hide + animation
        /*            Debug.Log("hittttttttttt");



                 if(c.gameObject.CompareTag("Hero") && HasFall && !DieOnce){ // not the fucking bubble
                    Debug.Log("firstttttttt");
                    string tag = c.gameObject.tag; // just hide it
                    Debug.Log("Link Hit "+ tag); 
                    DamageAnimation.Play();
                    DamageBoss(10);
                    Instantiate(HitAnimation, fireBall.transform.position, transform.rotation);
                    // explosionAnimation.Play();
                 }

                if(c.gameObject.CompareTag("obstacle") && OnTarget && EnterOnce){ // not the fucking bubble
                    Debug.Log("seccccccccccccccccccc");
                    EnterOnce=false;
                    fireBall.gameObject.SetActive(false);
                    Instantiate(HitAnimation, fireBall.transform.position, transform.rotation);
                 }*/


    }

    IEnumerator InitialWaitTimeFun(float time)  { // if in range will wait this amount of time (to avoid the spamming)
        yield return new WaitForSeconds(time);
        InitialWaitTime=true;
      
    }

    public void ArowHitFireBall(){
        
        if (!OnTarget)
        {
            animator.Play("fall");
            HasFall = true; // will be grounded and no bubble is around him anymore
            isFallOnHead = true;
        }
    }

     public void PlayHitSound(){
        FireBallHitSound.Play();
        // animator.Play("fall");
        // HasFall=true; // will be grounded and no bubble is around him anymore
        // isFallOnHead=true;
    }

   

    




}
