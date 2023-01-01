using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{


	public float explosionRadius = 10f;
	public float explosionForce = 0.1f;

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


			// if (rb.gameObject.CompareTag("Bokoblin") || rb.gameObject.CompareTag("Moblin"))
			// 	rb.gameObject.GetComponent<Bokoblinagent>().TakeDamage(5);


			if (rb.gameObject.CompareTag("Bokoblin"))
			{
				MonoBehaviour w = rb.gameObject.GetComponent<MonoBehaviour>();
				string type = w.GetType().Name;
				if (type == "Bokoblinagent")
				{
					rb.gameObject.GetComponent<Bokoblinagent>().TakeDamage(10);
				}
				else
				{
					rb.gameObject.GetComponent<Moblinagent>().TakeDamage(10);

				}
			}
			if (rb.gameObject.CompareTag("Moblin"))
			{
				MonoBehaviour w = rb.gameObject.GetComponent<MonoBehaviour>();
				string type = w.GetType().Name;
				if (type == "Bokoblinagent")
				{
					rb.gameObject.GetComponent<Bokoblinagent>().TakeDamage(10);
				}
				else
				{
					rb.gameObject.GetComponent<Moblinagent>().TakeDamage(10);

				}
			}
            if (rb.gameObject.CompareTag("Boss2"))
            {
                if (rb.gameObject.GetComponent<boss2Script>().getPhase())
                {
                    Debug.Log("the dmg is doubled");
                    rb.gameObject.GetComponent<boss2Script>().TakeDamage(5);
                    return;
                }
                else if (!rb.gameObject.GetComponent<boss2Script>().getShieldStatus())
                { // dbl the dmg
                    rb.gameObject.GetComponent<boss2Script>().TakeDamage(5 * 2);
                }
            }




            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
		}

		Destroy(gameObject);
	}

}
