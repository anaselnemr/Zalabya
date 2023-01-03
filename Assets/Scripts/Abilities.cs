using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
	public Image AbilityImg;
	public Sprite spriteBomb;
	public Sprite spriteMag;
	public Sprite spriteStatus;


	public LayerMask whatisplayerLayer;
	public LayerMask whatisgroundlayer;


	private void Start()
	{
		//Text.text = "Bomb";
		AbilityImg.sprite = spriteBomb;
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

			if (vector.magnitude > 4f)
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
			//	Text.text = "Bomb";
			AbilityImg.sprite = spriteBomb;

		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			selectedAbility = 3;
			//	Text.text = "Magnesis";
			AbilityImg.sprite = spriteMag;

		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			selectedAbility = 4;
			//	Text.text = "Stasis";
			AbilityImg.sprite = spriteStatus;

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
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 100, out hit, 100f, ~(whatisplayerLayer + whatisgroundlayer)))
			{
				Debug.Log("Object is " + hit.collider.name);
				object_grabbed = hit.collider.GetComponent<Rigidbody>();
				object_grabbed.GetComponent<Renderer>().material.color = (Color.red + Color.yellow) / 2;
				object_grabbed.isKinematic = false;
				object_grabbed.useGravity = true;
				object_grabbed.mass = 1;

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
		if (object_grabbed != null && Input.GetKeyUp(KeyCode.Q))
		{
			Release();
		}
		if (selectedAbility == 4 && Input.GetKeyDown(KeyCode.Q))
		{
			// TODO: Stasis
			activeStasis = true;
		}


	}

	void Release()
	{
		if (object_grabbed != null)
		{
			object_grabbed.mass = 1000;
			StartCoroutine(Returntonormal());



		}

		if (currentBomb != null)
			currentBomb.GetComponent<Bomb>().Explode();

		activeStasis = false;

	}
	IEnumerator Returntonormal()
	{
		yield return new WaitForSeconds(2);
		object_grabbed.isKinematic = true;
		object_grabbed.useGravity = false;
		object_grabbed.GetComponent<Renderer>().material.color = Color.white;
		object_grabbed = null;



	}


}
