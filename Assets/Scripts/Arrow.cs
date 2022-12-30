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
	private void OnTriggerEnter(Collider c)
	{
		// if the arrow hits a collider
		Destroy(this);

		//   if hit enemy
		if (c.gameObject.CompareTag("Bokoblin"))
		{
			c.gameObject.GetComponent<Bokoblinagent>().TakeDamage(5);
		}
		if (c.gameObject.CompareTag("Moblin"))
		{
			c.gameObject.GetComponent<Moblinagent>().TakeDamage(5);
		}



	}
}
