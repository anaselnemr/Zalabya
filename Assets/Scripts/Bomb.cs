using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{


	public float explosionRadius = 10f;
	public float explosionForce = 1000f;

	private bool isExploded = false;

	void Start()
	{

	}

	public void Explode()
	{

		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach (Collider nearbyObject in colliders)
		{
			Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
			if (rb == null) continue;

			rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
		}

		Destroy(gameObject);
	}

}
