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
			animator.StopPlayback();
			isClimbing = false;
			return;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && isWallFront())
		{
			Debug.Log("isClmbing true");
			isClimbing = true;
		}


		if (isClimbing)
		{

			if (Input.GetKey("w"))
			{
				animator.PlayInFixedTime("climb_up");
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
			// can be because he reached the top
			RaycastHit hit;
			if (isClimbing && Physics.Raycast(transform.position, transform.forward, out hit, 1f))
			{
				if (hit.transform.CompareTag("Wall"))
				{
					Debug.Log("Over Edge");
					animator.PlayInFixedTime("over_edge");
					transform.position += new Vector3(0, 2.5f, 0);
					transform.Translate(transform.forward * 1f);

				}
			}

			isClimbing = false;
			Debug.Log("isClmbing false no Wall");
			animator.StopPlayback();

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
