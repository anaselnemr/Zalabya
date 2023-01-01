using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class Enemy1_idle : StateMachineBehaviour
{
	NavMeshAgent agent = null;
	GameObject player = null;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//  TODO: move agent init to start() if exist
		agent = animator.GetComponent<NavMeshAgent>();

		// get a reference to the player from the starter assset
		player = GameObject.FindGameObjectWithTag("Player");
		agent.SetDestination(player.transform.position);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			animator.SetTrigger("attack");
		}
		else
		{
			// follow the player
			agent.SetDestination(player.transform.position);
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
