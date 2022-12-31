using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //   Debug.Log("in colidddddde");
    }

     void OnCollisionEnter(Collision c){
        Debug.Log("in colidddddde");
        // if(c.gameObject.CompareTag("ground")){// i hit obst 1 thus will decrease the health by 1 and destroy the obs
        //     Destroy(c.gameObject); 
        // }
     }
}
