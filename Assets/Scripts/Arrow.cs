using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ParticleSystem arrowparticles;

    private void Start()
    {
        // remove the arrow after x seconds
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        Instantiate(arrowparticles, transform.position, transform.rotation);
    }


    private void OnTriggerEnter(Collider c)
    {
        // if the arrow hits a collider
        Destroy(this);

        //   if hit enemy
        if (c.gameObject.CompareTag("Bokoblin"))
        {
            MonoBehaviour w = c.gameObject.GetComponent<MonoBehaviour>();
            string type = w.GetType().Name;
            if (type == "Bokoblinagent")
            {
                c.gameObject.GetComponent<Bokoblinagent>().TakeDamage(5);
            }
            else
            {
                c.gameObject.GetComponent<Moblinagent>().TakeDamage(5);

            }
        }
        if (c.gameObject.CompareTag("Moblin"))
        {
            MonoBehaviour w = c.gameObject.GetComponent<MonoBehaviour>();
            string type = w.GetType().Name;
            if (type == "Bokoblinagent")
            {
                c.gameObject.GetComponent<Bokoblinagent>().TakeDamage(5);
            }
            else
            {
                c.gameObject.GetComponent<Moblinagent>().TakeDamage(5);

            }
        }
        if (c.gameObject.CompareTag("Boss1"))
        {
            c.gameObject.GetComponent<bossScript>().TakeDamage(5);
        }
        if (c.gameObject.CompareTag("fireBall")) // boss1
        {
            c.gameObject.transform.parent.GetComponent<bossScript>().ArowHitFireBall();
        }
        if (c.gameObject.CompareTag("Boss2"))
        {
            if (c.gameObject.GetComponent<boss2Script>().getPhase())
            {
                Debug.Log("the dmg is doubled");
                c.gameObject.GetComponent<boss2Script>().TakeDamage(5);
                return;
            }
            else if (!c.gameObject.GetComponent<boss2Script>().getShieldStatus())
            { // dbl the dmg
                c.gameObject.GetComponent<boss2Script>().TakeDamage(5 * 2);
            }
        }

    }
}
