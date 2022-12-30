using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
	public Transform bombPoint;
	public GameObject bombPrefab;
	public float bombThrowForce = 10f;
	private GameObject currentBomb;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (currentBomb == null)
			{
				GameObject bomb = Instantiate(bombPrefab, bombPoint.position, bombPoint.rotation);
				bomb.GetComponent<Rigidbody>().AddForce(bombPoint.forward * bombThrowForce + bombPoint.up, ForceMode.VelocityChange);
				currentBomb = bomb;
			}
			else
			{
				// Explode
				currentBomb.GetComponent<Bomb>().Explode();
			}


		}
	}
}
