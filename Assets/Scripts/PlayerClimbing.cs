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

	Animator animator;
	// Start is called before the first frame update
	void Start()
	{
		wall = GameObject.FindGameObjectWithTag("Wall");
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{

		// Debug.Log(wall.transform.forward);

		// Debug.DrawRay(point.transform.position, point.transform.forward * 1000, Color.red);

		if (Input.GetKeyDown(KeyCode.LeftShift) && isClimbing)
		{
			Debug.Log("isClmbing false");
			isClimbing = false;
			return;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && isWallFront())
		{
			Debug.Log("isClmbing true");
			isClimbing = true;
			transform.GetComponent<ThirdPersonController>().Gravity = 0;
		}


		if (isClimbing)
		{
			animator.PlayInFixedTime("climb_up");

			if (Input.GetKey("w"))
			{
				transform.position += point.transform.up * Time.deltaTime * UpwardSpeed;
			}

			if (Input.GetKey("s"))
			{
				transform.position += -point.transform.up * Time.deltaTime * UpwardSpeed;
			}

			if (Input.GetKey("d"))
			{
				// transform.position += -point.transform.forward * Time.deltaTime * UpwardSpeed;
				transform.Translate(wall.transform.right * Time.deltaTime * UpwardSpeed);
			}

			if (Input.GetKey("a"))
			{
				// transform.position += point.transform.forward * Time.deltaTime * UpwardSpeed;
				transform.Translate(-wall.transform.right * Time.deltaTime * UpwardSpeed);
			}

		}

		if (!isWallFront())
		{
			isClimbing = false;
			Debug.Log("isClmbing false");
			transform.GetComponent<ThirdPersonController>().Gravity = -15f;
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
