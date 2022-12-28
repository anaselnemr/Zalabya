using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	private void Start()
	{
		// remove the arrow after x seconds
		Destroy(gameObject, 10);
	}
	private void OnTriggerEnter(Collider other)
	{
		// if the arrow hits a collider
		Destroy(transform.GetComponent<Rigidbody>());

		//   if hit enemy
		if (other.gameObject.tag == "enemy")
		{
			// destroy the enemy
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
