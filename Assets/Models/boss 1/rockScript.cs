using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rockScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public Transform rock;
    private NavMeshAgent navMeshAgent;
    // public bool attacking=false ; 
    // public Transform point1;
    // public Transform point2;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        navMeshAgent.SetDestination(target.position);
     
    }

    // Update is called once per frame
    void Update()
    {
        // if(attacking){ // on it way to the player

        // }
        // else{ 

        // }
        // rock = GameObject.FindGameObjectsWithTag("rock"); // will handle to destroy what behind me
        // rock.transform.position = rock.transform.position + target.transform.position;
        
      
        
    }
}
