using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class enemyagent : MonoBehaviour
{

    public Transform target;
    private NavMeshAgent navMeshAgent;
    public Transform point1;
    public Transform point2;
    public bool onTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(point2.position);
    }

    // Update is called once per frame
    void Update()
    {

        if (!onTarget)
        {

            // navMeshAgent.SetDestination(target.position); // to follow the target
            Debug.DrawRay(transform.position, transform.forward * 1000, Color.red);
            if (Vector3.Distance(transform.position, point1.position) < 3)
            { // between 2 points will move 
                navMeshAgent.SetDestination(point2.position);
            }
            else if (Vector3.Distance(transform.position, point2.position) < 3)
            {
                navMeshAgent.SetDestination(point1.position);
            }

        }

        /*  if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo, 10))
          {
              if (hitinfo.collider.tag == "Player")
              {
                  navMeshAgent.SetDestination(target.position);
                  Debug.DrawRay(transform.position, transform.forward * 1000, Color.green);
              }
          }*/

            if (Vector3.Distance(transform.position, target.position) < 3 || onTarget == true)
            {
               onTarget = true;
                navMeshAgent.SetDestination(target.position);
            }
        
        
    }
    }


