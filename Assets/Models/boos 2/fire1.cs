using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      public void OnTriggerEnter(Collider c) { // here will break the ball w5las hide + animation
        string tag = c.gameObject.tag; // just hide it
        Debug.Log("ball hit something"+tag); 
      
      
         if(c.gameObject.CompareTag("Hero")){ // not the fucking bubble
            // explosionAnimation.Play();
            c.gameObject.transform.parent.gameObject.transform.parent.GetComponent<ThirdPersonController>().TakeDamage(2);
            // StartCoroutine("WaitExplode");
        }
    }

}
