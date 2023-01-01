using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour { // Player Script

    [Range(0,50)]
    public float speed = 1;
    public float sensitivity = 1.0f;
    private float MyHealth = 80 ;

    // Update is called once per frame
    void Update() {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * speed);
        float mouseDeltaX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseDeltaY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the camera transform around the y-axis
        transform.Rotate(0, mouseDeltaX, 0);

        // Clamp the pitch angle to prevent the camera from flipping
        // float pitch = transform.localEulerAngles.x - mouseDeltaY;
        // pitch = Mathf.Clamp(pitch, -89.0f, 89.0f);

        // Rotate the camera transform around the x-axis
        // transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0);
    }

    // void OnCollisionEnter(Collision c){

    //     Debug.Log("in colidddddde");
    //     // if(c.gameObject.CompareTag("ground")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
    //     //     Destroy(rock); 
    //     // }
    //  }

    //  void OnTriggerEnter(Collider c) {
    //     // This function is called when the collider enters the trigger
    //      if(c.gameObject.CompareTag("ground")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
    //         Destroy(rock); 
    //     }
    //     Debug.Log("Collider entered the trigger");
    // }


    //  void OnCollisionEnter(Collision c){
  
    //       string tag = c.gameObject.tag;
    //     // Debug.Log("i have colided with something"+tag); 
    //    if(c.gameObject.CompareTag("fireBall")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
    //       MyHealth-=4 ; // fire ball will dmg by 4
    //       Debug.Log("i have been dmg: "+ MyHealth);
    //     }
    //  }

     public void OnTriggerEnter(Collider c) {
         if(c.gameObject.CompareTag("fireBall")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
          MyHealth-=4 ; // fire ball will dmg by 4
          Debug.Log("i have been dmg: "+ MyHealth);
        }
    }
    
}
