using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
/*using UnityEngine.UIElements;
*/
public class Moblinagent : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    private Animator a;
    //Patroling
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public Slider Healthbar;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    private static bool chased = false;
    private bool once = true;
    private int r;
    public int slowdamage;
    public int fastdamage;
    private bool notagain = true;

    private void Start()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        a = this.GetComponent<Animator>();
    }

    private void Update()
    {
        Healthbar.value = health;
        //Check for sight and attack range
        if(!isPlaying("Fast attack") && !isPlaying("Slow attack"))
            once = true;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange && !chased && !isPlaying("Idle")) Patroling();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        if ((playerInSightRange && !playerInAttackRange) || chased) ChasePlayer();

    }

    private void Patroling()
    {
/*        Debug.Log("Patrol");
*/
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
/*        Debug.Log(distanceToWalkPoint);
*/
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 5f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
/*        Debug.Log("SearchWalkPoint");
*/
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        Debug.Log("Chase Player");
        if (gameObject.tag == "Moblin " && notagain)
        {
            notagain = false;
            a.Play("Run");
        }
        chased = true;
        agent.SetDestination(player.position);
        transform.LookAt(player);

    }

    private void AttackPlayer()
    {
        Debug.Log("Attack Player");
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked && health>0)
        {
            int r = Random.Range(0, 2);
            if(r == 0)
            a.PlayInFixedTime("Fast attack");
            else
            a.PlayInFixedTime("Slow attack");

            alreadyAttacked = true; 
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void OnTriggerStay(Collider c)
    {


        if (c.gameObject.CompareTag("Hero") && isPlaying("Fast attack") && once)
        {
            once = false;
            Debug.Log(c.gameObject.transform.parent.gameObject.transform.parent.name);
            c.gameObject.transform.parent.gameObject.transform.parent.GetComponent<ThirdPersonController>().TakeDamage(fastdamage);
        }
        if (c.gameObject.CompareTag("Hero") && isPlaying("Slow attack") && once)
        {
            once = false;
            Debug.Log(c.gameObject.transform.parent.gameObject.transform.parent.name);
            c.gameObject.transform.parent.gameObject.transform.parent.GetComponent<ThirdPersonController>().TakeDamage(slowdamage);
        }

    }
    bool isPlaying(string stateName)
    {
        if (a.GetCurrentAnimatorStateInfo(0).IsName(stateName) && a.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        int i = Random.Range(0, 2);
        if (health <= 0)
        {

            a.PlayInFixedTime("Die");
            StartCoroutine(DestroyEnemy());
        }
        else
        {
            if (i == 0)
            {
                a.PlayInFixedTime("damage");
                Debug.Log("HEREAFDAFASDFASDF");
            }
            else
            {
                Debug.Log("NOT ASFBASDHFADSFSADs");

                a.PlayInFixedTime("Hit");
            }
        }
        ChasePlayer();
    }
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }


}





/*

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting.Antlr3.Runtime;

public class Player1 : MonoBehaviour
{
    public GameObject PhonePlayPanel;
    public GameObject g;
    public GameObject pausePanel;
    public Rigidbody r;
    public bool onGround = true;
    public bool invincible = false;

    private int health = 5;
    private int ability = 10;
    public int score = 0;
    public TMP_Text scores;
    public TMP_Text healths;
    public TMP_Text abilitys;
    public Animator anim;

    public AudioSource A;
    public AudioSource collision;
    public AudioSource pause;
    public AudioSource healthmusic;
    public AudioSource abilitymusic;

    private float volume;


    *//*    public TMP_Text score;
    *//*
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("Hello Awake");
    }
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            PhonePlayPanel.SetActive(true);
        else
            PhonePlayPanel.SetActive(false);

        if (PlayerPrefs.HasKey("volume"))
        {
            volume = (PlayerPrefs.GetFloat("volume"));

            A.Play();
            A.volume = volume;
        }

        pausePanel.SetActive(false);
        r.GetComponent<Rigidbody>();
        PlayerPrefs.SetInt("Scote", score);
        PlayerPrefs.SetInt("Ability", ability);
        anim.GetComponent<Animator>();


    }
    void FixedUpdate()
    {


        // transform.position = transform.position + new Vector3(0.01f, 0, 0);
        *//*        Debug.Log(transform.position);
        */        /*        transform.Translate(new Vector3(0.01f, 0, 0));
                *//*     // transform.Translate(Vector3.right * Time.deltaTime * speed);
        *//*        r.AddForce(new Vector3(0.01f,0,0));
                g.transform.Translate(new Vector3(0.1f, 0, 0));
                //transform.Translate(Vector3.left);
                //Debug.Log("Hello Update
                l.transform.Rotate(Vector3.up * Time.deltaTime * 500f);*/
/*        if ((Input.GetKey("up")  || Input.GetKey("w")) && Time.timeScale == 1)
        {
            transform.position = transform.position + new Vector3(0, 0, 0.1f);
        }*//*

if (Input.GetKey("space") && onGround && ability >= 1 && Time.timeScale == 1)
{
    r.AddForce(Vector3.up * 1600f);
    //anim.Play("Running Jump");
    anim.PlayInFixedTime("Jump");

    *//*                transform.position = transform.position + new Vector3(0, 7, 0);
    *//*
    onGround = false;
    abilityminus(1);
    *//*                StartCoroutine(stopjump());
    *//*
}
if ((Input.GetKey("left") || Input.GetKey("a")) && Time.timeScale == 1)
{
    anim.PlayInFixedTime("Left");

    if (transform.position.x > -20)
        transform.position = transform.position + new Vector3(-0.6f, 0, 0);
}

if ((Input.GetKey("right") || Input.GetKey("d")) && Time.timeScale == 1)
{
    anim.PlayInFixedTime("Right");
    if (transform.position.x < 20)
        transform.position = transform.position + new Vector3(0.6f, 0, 0);
}
transform.position = transform.position + new Vector3(0, 0, 0.5f);

}
void Update()
{
if ((Input.GetKeyDown("g") && Time.timeScale == 1))
{
    healthadd(1);
}
if ((Input.GetKeyDown("h") && Time.timeScale == 1))
{
    healthminus(1);
}
if ((Input.GetKeyDown("j") && Time.timeScale == 1))
{
    abilityadd(1);
}
if ((Input.GetKeyDown("k") && Time.timeScale == 1))
{
    abilityminus(1);
}
if ((Input.GetKeyDown("l") && Time.timeScale == 1))
{
    invincible = !invincible;
}

if (Input.GetKeyDown(KeyCode.Escape))
{
    if (Time.timeScale == 1)
    {
        A.Pause();
        pause.Play();
        pause.volume = volume;
        Time.timeScale = 0;
        pausePanel.SetActive(true);

    }
    else
    {
        pause.Stop();
        A.Play();
        A.volume = volume;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
PlayerPrefs.SetFloat("volume", volume);
PlayerPrefs.SetFloat("Playerpos", transform.position.z);

if (PlayerPrefs.HasKey("Abilityafter"))
{
    int w = PlayerPrefs.GetInt("Abilityafter");
    PlayerPrefs.DeleteKey("Abilityafter");
    abilityminus(w);
}

}
private void OnCollisionEnter(Collision c)
{
if (c.gameObject.CompareTag("Tile"))
{
    onGround = true;
}
if (c.gameObject.CompareTag("Red"))
{
    if (health < 5)
    {
        Destroy(c.gameObject);
        healthadd(1);
    }
}
if (c.gameObject.CompareTag("Yellow"))
{
    if (ability < 10)
    {
        Destroy(c.gameObject);
        abilityadd(1);
    }
}
if (c.gameObject.CompareTag("1") && !invincible)
{


    if (transform.position.y > 11)
    {
        scorecheck(3);
        Destroy(c.gameObject);
    }
    else
    {
        healthminus(3);
        Destroy(c.gameObject);
        collision.Play();
        collision.volume = volume;
        GameObject.Find("Main Camera").GetComponent<CameraControl>().shakeDuration = 0.25f;
    }
}
if (c.gameObject.CompareTag("2") && !invincible)
{

    if (transform.position.y > 11)
    {
        scorecheck(2);
        Destroy(c.gameObject);
    }
    else
    {
        healthminus(2);
        Destroy(c.gameObject);
        collision.Play();
        collision.volume = volume;
        GameObject.Find("Main Camera").GetComponent<CameraControl>().shakeDuration = 0.25f;
    }
}

if (c.gameObject.CompareTag("3") && !invincible)
{
    if (transform.position.y > 11)
    {
        scorecheck(1);
        Destroy(c.gameObject);

    }
    else
    {
        healthminus(1);
        Destroy(c.gameObject);
        collision.Play();
        collision.volume = volume;
        GameObject.Find("Main Camera").GetComponent<CameraControl>().shakeDuration = 0.25f;
    }
}

}
*//*    IEnumerator stopjump()
    {
        yield return new WaitForSeconds(0.4f);
        transform.position = transform.position + new Vector3(0, -7, 0);
        yield return new WaitForSeconds(0.4f);
        transform.position = transform.position + new Vector3(0, 0, 0);

    }*//*
public void RightButton()
{
    if (Time.timeScale == 1)
    {
        anim.PlayInFixedTime("Right");
        if (transform.position.x + 10 <= 20)
            transform.position = new Vector3(transform.position.x + 15, transform.position.y, transform.position.z);
    }
}
public void LeftButton()
{
    if (Time.timeScale == 1)
    {

        anim.PlayInFixedTime("Left");

        if (transform.position.x - 10 >= -25)
            transform.position = new Vector3(transform.position.x - 15, transform.position.y, transform.position.z);
    }
}
public void JumpButton()
{

    if (onGround && ability >= 1 && Time.timeScale == 1)
    {
        r.AddForce(Vector3.up * 1600f);
        //anim.Play("Running Jump");
        anim.PlayInFixedTime("Jump");

        *//*                transform.position = transform.position + new Vector3(0, 7, 0);
        *//*
        onGround = false;
        abilityminus(1);
    }
}
public void PauseButton()
{
    if (Time.timeScale == 1)
    {
        A.Pause();
        pause.Play();
        pause.volume = volume;
        Time.timeScale = 0;
        pausePanel.SetActive(true);

    }
    else
    {
        pause.Stop();
        A.Play();
        A.volume = volume;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
private void healthadd(int x)
{
    if (health == 5)
        health = 5;
    else
    {
        healthmusic.Play();
        healthmusic.volume = volume;
        health += x;
    }
    healths.text = "" + health;
    return;


}
private void healthminus(int x)
{
    health -= x;
    if (health <= 0)
    {
        health = 0;
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(2); //Game over
    }
    healths.text = "" + health;
    return;

}
private void abilityadd(int x)
{
    if (ability == 10)
        ability = 10;
    else
    {
        abilitymusic.Play();
        abilitymusic.volume = volume;
        ability += x;
    }
    abilitys.text = "" + ability;
    PlayerPrefs.SetInt("Ability", ability);
    return;
}
private void abilityminus(int x)
{
    ability -= x;
    if (ability <= 0)
        ability = 0;
    abilitys.text = "" + ability;
    PlayerPrefs.SetInt("Ability", ability);

}
private void scorecheck(int x)
{
    score += x;
    scores.text = "" + score;
    return;
}
private void OnTriggerEnter(Collider c)
{
    *//*        Destroy(c.gameObject);
            Destroy(g);*//*

}
private void OnTriggerStay(Collider c)
{


}
private void OnTriggerExit(Collider c)
{

}

public void buttonfn()
{
    *//*        Debug.Log("HELLLLLLLOOOOOOOOOOOOO");
    *//*
}


}
*/
