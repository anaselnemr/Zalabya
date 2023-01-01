using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem explosionAnimation ; // when hitting somthing will play this
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    //     if (Input.GetKeyDown(KeyCode.Space))
    // {
    //     // Play the particle system if it is not already playing
      
    //         explosionAnimation.Play();
      
    // }
        
    }

    public void OnTriggerEnter(Collider c) { // here will break the ball w5las hide + animatio
        string tag = c.gameObject.tag; // just hide it
        Debug.Log("ball hit something"+tag); 
        //  c.gameObject.transform.parent.gameObject.PlayHitSound();
    
         if(c.gameObject.CompareTag("Hero")){ // not the fucking bubble /// play sound damge
            gameObject.SetActive(false); // hide the ball 5las
            c.gameObject.transform.parent.gameObject.transform.parent.GetComponent<ThirdPersonController>().TakeDamage(4);
            Instantiate(explosionAnimation, transform.position, transform.rotation);
            // StartCoroutine("WaitExplode");
        }

      if(c.gameObject.CompareTag("obstacle")){ // not the fucking bubble
            gameObject.SetActive(false); // hide the ball 5las
            Instantiate(explosionAnimation, transform.position, transform.rotation);
        }


        // if(c.gameObject.CompareTag("obstacle")){ // not the fucking bubble
        //     gameObject.SetActive(false); // hide the ball 5las
        //     Instantiate(explosionAnimation, transform.position, transform.rotation);
        // }
      

      
    }

    //  IEnumerator WaitExplode()  { // if in range will wait this amount of time (to avoid the spamming)
    //     yield return new WaitForSeconds(0.2f);
    //     gameObject.SetActive(false);
    // }

}
