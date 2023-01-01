using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Abilities : MonoBehaviour
{
	public int selectedAbility = 1; // One based, Remote Bombs, Cryonis, Magnesis, and Stasis

	public Transform bombPoint;
	public GameObject bombPrefab;
	public float bombThrowForce = 10f;
	private GameObject currentBomb;
	// =====
	private Rigidbody object_grabbed;
	public GameObject chestpoint;
	// 
	public static bool activeStasis = false;
	public TMP_Text Text;
	private void Start()
	{
        Text.text = "Bomb";
    }
	private void FixedUpdate()
	{
		if (object_grabbed)
		{
			Vector3 vector = (chestpoint.transform.position + chestpoint.transform.forward * 4f) - object_grabbed.transform.position;
			Vector3 torque = new Vector3(-object_grabbed.rotation.x, -object_grabbed.rotation.y, -object_grabbed.rotation.z) * Mathf.Deg2Rad;

			object_grabbed.velocity = Vector3.zero;
			object_grabbed.angularVelocity = Vector3.zero;

			object_grabbed.AddForce(vector * 100f);
			object_grabbed.AddTorque(torque * 20f);

			if (vector.magnitude > 3f)
			{
				Release();
			}

		}
	}

	private void Update()
	{
		int prvSelectedAbility = selectedAbility;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			selectedAbility = 1;
			Text.text = "Bomb";
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			selectedAbility = 3;
            Text.text = "Magnesis";

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			selectedAbility = 4;
            Text.text = "Stasis";

        }

        if (prvSelectedAbility != selectedAbility)
		{
			Release();
		}


		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);

		if (selectedAbility == 3 && Input.GetKeyDown(KeyCode.Q))
		{
			RaycastHit hit;
			// if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f))
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 100, out hit))
			{
				Debug.Log("Object is " + hit.collider.name);
				object_grabbed = hit.collider.GetComponent<Rigidbody>();
			}

		}


		if (selectedAbility == 1 && Input.GetKeyUp(KeyCode.Q))
		{
			if (currentBomb == null)
			{
				this.GetComponent<Animator>().PlayInFixedTime("Throwing");
				GameObject bomb = Instantiate(bombPrefab, bombPoint.position, bombPoint.rotation);
				bomb.GetComponent<Rigidbody>().AddForce(bombPoint.forward * bombThrowForce + bombPoint.up, ForceMode.VelocityChange);
				currentBomb = bomb;
			}
			else
			{
				// Explode
				Release();
			}
		}

		if (selectedAbility == 4 && Input.GetKeyDown(KeyCode.Q))
		{
			// TODO: Stasis
			activeStasis = true;
		}


	}

	void Release()
	{
		object_grabbed = null;

		if (currentBomb != null)
			currentBomb.GetComponent<Bomb>().Explode();

		activeStasis = false;

	}


}
