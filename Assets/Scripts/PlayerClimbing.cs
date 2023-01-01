using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{

	public float range = 7f;

	public float UpwardSpeed = 3.3f;

	public Transform point;
	public static bool isClimbing = false;

	private GameObject wall;

<<<<<<< HEAD
=======

>>>>>>> mine/test
	Animator animator;
	// Start is called before the first frame update
	void Start()
	{
		wall = GameObject.FindGameObjectWithTag("Wall");
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{


		if (Input.GetKeyUp(KeyCode.LeftShift) && isClimbing)
		{
			Debug.Log("isClmbing false");
			isClimbing = false;
			animator.PlayInFixedTime("Idle Walk Run Blend");
		}

		if (!isClimbing && Input.GetKey(KeyCode.LeftShift) && isWallFront())
		{
			Debug.Log("isClmbing true");
			isClimbing = true;
		}


		if (isClimbing)
		{

			if (Input.GetKey("w"))
			{
				animator.Play("climb_up");
				transform.position += point.transform.up * Time.deltaTime * UpwardSpeed;
			}
			else
			if (Input.GetKey("s"))
			{
				animator.Play("climb_down");
				transform.position += -point.transform.up * Time.deltaTime * UpwardSpeed;
			}
			else
			if (Input.GetKey("d"))
			{
				animator.Play("climb_right");
				transform.Translate(wall.transform.right * Time.deltaTime * UpwardSpeed);
			}
			else
			if (Input.GetKey("a"))
			{
				animator.Play("climb_left");
				transform.Translate(-wall.transform.right * Time.deltaTime * UpwardSpeed);
			}
			else
			{
				animator.Play("climb_idle");
			}

		}

		if (!isWallFront())
		{
			// can be because he reached the top
			RaycastHit hit;
			if (isClimbing && Physics.Raycast(transform.position, transform.forward, out hit, 1f))
			{
				if (hit.transform.CompareTag("Wall"))
				{
					Debug.Log("Over Edge");
					animator.Play("over_edge");
					transform.position += new Vector3(0, 2.5f, 0);
					transform.Translate(point.transform.forward * 0.5f, Space.World);

				}
			}

			isClimbing = false;
			// Debug.Log("isClmbing false no Wall");
		}

	}


	bool isWallFront()
	{
		RaycastHit hit;
		if (Physics.Raycast(point.transform.position, point.transform.forward, out hit, range))
		{
			// Debug.Log("HIT with " + hit.transform.name);

			if (hit.transform.CompareTag("Wall"))
			{
				return true;
				// Debug.Log("Wall");
				// isClimbing = true;
				// transform.position += Vector3.up * Time.deltaTime * UpwardSpeed;
			}
		}
		return false;
	}

}
