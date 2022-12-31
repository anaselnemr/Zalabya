using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{

	public float range = 10f;

	public float UpwardSpeed = 3.3f;

	public Transform point;
	public static bool isClimbing = false;

	private GameObject wall;

	// Start is called before the first frame update
	void Start()
	{
		wall = GameObject.FindGameObjectWithTag("Wall");
	}

	private void FixedUpdate()
	{

		// Debug.Log(wall.transform.forward);

		// Debug.DrawRay(point.transform.position, point.transform.forward * 1000, Color.red);


		if (Input.GetKeyDown(KeyCode.LeftShift) && isWallFront())
		{
			Debug.Log("isClmbing true");
			isClimbing = true;
		}
		else
		if (Input.GetKeyDown(KeyCode.LeftShift) && isClimbing)
		{
			Debug.Log("isClmbing false");
			isClimbing = false;
		}

		if (!isWallFront())
		{
			isClimbing = false;
		}

		if (isClimbing)
		{

			if (Input.GetKey("w"))
			{
				transform.position += Camera.main.transform.up * Time.deltaTime * UpwardSpeed;
			}

			if (Input.GetKey("s"))
			{
				transform.position += -Camera.main.transform.up * Time.deltaTime * UpwardSpeed;
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

	}


	bool isWallFront()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
		{
			Debug.Log("HIT with " + hit.transform.name);

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
